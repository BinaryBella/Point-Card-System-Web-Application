using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;

namespace Point_Card_System.Pages
{
    public partial class CustomerLedger : System.Web.UI.Page
    {

        private void LoadTransactionData(string phoneNumber)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_Get_CustomerTransactions_ByPhone", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gv_Transactions.DataSource = dt;
                gv_Transactions.DataBind();
            }
        }
        string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize page values here
                LoadAllPhoneNumbersForJS();

                string phoneNumber = Request.QueryString["Phone"];
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    txtPhoneSearch.Text = phoneNumber;
                    LoadCustomerDetails(phoneNumber);
                    LoadTransactionData(phoneNumber);
                }
            }
            else
            {
                // Handle postback from phone search textbox
                string eventTarget = Request["__EVENTTARGET"];
                if (eventTarget == txtPhoneSearch.UniqueID)
                {
                    string phoneNumber = txtPhoneSearch.Text.Trim();
                    if (!string.IsNullOrEmpty(phoneNumber))
                    {
                        LoadCustomerDetails(phoneNumber);
                        LoadTransactionData(phoneNumber);
                    }
                }
            }
        }

        private void LoadAllPhoneNumbersForJS()
        {
            List<string> phoneNumbers = new List<string>();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT Mobile_Number FROM Customer_Master WHERE Mobile_Number IS NOT NULL AND Mobile_Number != ''", con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    phoneNumbers.Add(reader["Mobile_Number"].ToString());
                }
            }

            // Store phone numbers in hidden field for JavaScript access
            hfAllPhoneNumbers.Value = string.Join(",", phoneNumbers);
        }

        private void LoadCustomerDetails(string phoneNumber)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_Get_CustomerDetails_ByPhoneNumber", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtName.Text = reader["Customer_Name"].ToString();
                    txtAddress.Text = reader["Address1"].ToString();
                    txtPoints.Text = reader["Available_Points"].ToString();

                    // Load points analysis if you have the stored procedure for it
                    LoadPointsAnalysis(phoneNumber);
                }
                else
                {
                    // Customer not found - show SweetAlert2 error
                    ClearCustomerFields();
                    ShowInvalidPhoneNumberAlert();
                }
                reader.Close();
            }
        }

        private void ShowInvalidPhoneNumberAlert()
        {
            string script = @"
                Swal.fire({
                    icon: 'error',
                    title: 'Invalid Phone Number',
                    text: 'Invalid or unregistered customer phone number.',
                    confirmButtonColor: '#d33'
                }).then(function() {
                    document.getElementById('" + txtPhoneSearch.ClientID + @"').value = '';
                    document.getElementById('" + txtPhoneSearch.ClientID + @"').focus();
                });";

            ClientScript.RegisterStartupScript(this.GetType(), "InvalidPhoneAlert", script, true);
        }

        private void LoadPointsAnalysis(string phoneNumber)
        {
            // You can implement this method to load the points analysis data
            // This is a placeholder - adjust according to your database structure
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SP_Get_CustomerPointsAnalysis_ByPhone", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            lblTotalPoints.Text = reader["TotalPoints"]?.ToString() ?? "0.00";
                            lblAvailablePoints.Text = reader["AvailablePoints"]?.ToString() ?? "0.00";
                            lblRenewablePoints.Text = reader["RenewablePoints"]?.ToString() ?? "0.00";
                        }
                        else
                        {
                            // Set default values if no data found
                            lblTotalPoints.Text = "0.00";
                            lblAvailablePoints.Text = txtPoints.Text; // Use the available points from customer details
                            lblRenewablePoints.Text = "0.00";
                        }
                    }
                }
                catch (Exception)
                {
                    // If stored procedure doesn't exist, use basic values
                    lblTotalPoints.Text = txtPoints.Text;
                    lblAvailablePoints.Text = txtPoints.Text;
                    lblRenewablePoints.Text = "0.00";
                }
            }
        }

        private void ClearCustomerFields()
        {
            txtName.Text = "";
            txtAddress.Text = "";
            txtPoints.Text = "";
            lblTotalPoints.Text = "0.00";
            lblAvailablePoints.Text = "0.00";
            lblRenewablePoints.Text = "0.00";

            // Clear the GridView
            gv_Transactions.DataSource = null;
            gv_Transactions.DataBind();
        }
    }
}