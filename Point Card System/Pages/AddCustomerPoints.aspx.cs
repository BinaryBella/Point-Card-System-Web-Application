using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Point_Card_System.DAL;
using static Point_Card_System.DAL.CustomerController;

namespace Point_Card_System.Pages
{
    public partial class AddCustomerPoints1 : System.Web.UI.Page
    {
        CustomerController cuscon = new CustomerController();
        string branchcode = "";
        string userid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Session["Username"] as string;
            string password = Session["Password"] as string;
            if (username != null)
            {
                // If session exists, display the username or use it for some purpose
                get_branch_code(username, password);
            }
            else
            {
                // If session does not exist (user is not logged in), redirect to login page
                Response.Redirect("Login.aspx");
            }
        }

        void get_branch_code(string user, string password)
        {
            branchcode = cuscon.Get_customer_branchcode(user, password);
            userid = cuscon.Get_userid(user, password);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Example: Save entered data
            string phone = txtCustomerPhoneNo.Text;
            string name = txtCustomerName.Text;
            string invoiceNo = txtInvoiceNo.Text;
            string total = txtInvoiceTotal.Text;
            string points = txtPoints.Text;
            string branchid = branchcode;

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "Mobile_Number", phone },
                    { "Branch_ID", branchcode },
                    { "Created_Date", DateTime.Now },
                    { "invoice_amt", double.Parse(total)},
                    { "invoice_points", double.Parse(points)},
                    { "user", userid},
                    { "invoice_number", invoiceNo }
                };

                // Call the method to insert points
                cuscon.InsertPointsDetails(parameters);

                // If we reach this point without exception, consider it successful
                // Success - Show SweetAlert2 success message
                string script = "showSuccessAlert('Customer points added successfully.');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowSuccessAlert", script, true);

                // Clear form fields after successful save
                ClearFormFields();
            }
            catch (Exception ex)
            {
                // Exception occurred - Show SweetAlert2 error message
                string script = "showErrorAlert('Failed to add customer points. Error: " + ex.Message.Replace("'", "\\'") + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowErrorAlert", script, true);
            }
        }

        private void ClearFormFields()
        {
            txtCustomerPhoneNo.Text = "";
            txtCustomerName.Text = "";
            txtInvoiceNo.Text = "";
            txtInvoiceTotal.Text = "";
            txtPoints.Text = "";
            txtCustomerName.ForeColor = System.Drawing.Color.Black;
        }

        protected void txtInvoiceTotal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtInvoiceTotal.Text))
                {
                    double points = 0;
                    points = Convert.ToDouble(txtInvoiceTotal.Text) * (0.5 / 100);
                    txtPoints.Text = points.ToString("F2"); // Format to 2 decimal places
                }
                else
                {
                    txtPoints.Text = "";
                }
            }
            catch (Exception)
            {
                txtPoints.Text = "0.00";
            }
        }

        string cusid = "";
        string mobile;

        protected void txtCustomerPhoneNo_TextChanged(object sender, EventArgs e)
        {
            string phone = txtCustomerPhoneNo.Text;

            try
            {
                // Call the method and get the invoice details
                InvoiceDetails details = cuscon.get_cusname(phone);

                if (details != null && details.CustomerName != "0")
                {
                    txtCustomerName.Text = details.CustomerName;
                    mobile = details.MobileNumber;
                    cusid = details.CustomerId;
                    txtCustomerName.ForeColor = System.Drawing.Color.Blue;
                    txtInvoiceNo.Focus();
                }
                else
                {
                    txtCustomerName.Text = "";
                    txtCustomerName.ForeColor = System.Drawing.Color.Black;
                    // Clear other fields when customer is not found
                    txtInvoiceNo.Text = "";
                    txtInvoiceTotal.Text = "";
                    txtPoints.Text = "";

                    // Show SweetAlert2 warning message for invalid mobile number
                    string script = "showWarningAlert('Invalid mobile number or not registered');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowWarningAlert", script, true);
                }
            }
            catch (Exception ex)
            {
                txtCustomerName.Text = "Error retrieving customer details";
                txtCustomerName.ForeColor = System.Drawing.Color.Red;

                // Show error alert for customer lookup failure
                string script = "showErrorAlert('Error retrieving customer details: " + ex.Message.Replace("'", "\\'") + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowCustomerErrorAlert", script, true);
            }
        }
    }
}