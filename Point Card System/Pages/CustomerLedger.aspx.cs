// CustomerLedger.aspx.cs
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Point_Card_System.Pages
{
    public partial class CustomerLedger : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize page values here
               
                LoadPhoneNumberDropdown();
                string phoneNumber = Request.QueryString["Phone"];
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    LoadCustomerDetails(phoneNumber);
                    LoadTransactionData(phoneNumber);
                }
            }
        }

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


        private void LoadPhoneNumberDropdown()
        {
           

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT Mobile_Number FROM Customer_Master", con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                txt_SearchPhone.DataSource = reader;
                txt_SearchPhone.DataTextField = "Mobile_Number";
                txt_SearchPhone.DataValueField = "Mobile_Number";
                txt_SearchPhone.DataBind();

                txt_SearchPhone.Items.Insert(0, new ListItem("-- Select Phone Number --", ""));
            }
        }

        protected void txt_SearchPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPhone = txt_SearchPhone.SelectedValue;

            if (!string.IsNullOrEmpty(selectedPhone))
            {
                LoadCustomerDetails(selectedPhone);
                LoadTransactionData(selectedPhone);
            }
            else
            {
                ClearCustomerFields();
            }
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
                }
                else
                {
                    ClearCustomerFields();
                }

                reader.Close();
            }
        }

        private void ClearCustomerFields()
        {
            txtName.Text = "";
            txtAddress.Text = "";
            txtPoints.Text = "";
        }
    }
}