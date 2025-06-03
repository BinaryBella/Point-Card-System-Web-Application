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
                cuscon.InsertPointsDetails(parameters);
                Response.Write("Pints Added: " + name);

                //hfCustomerId.Value = string.Empty;
                //textbox_Name.Text = "";

                //get_customer_code();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        protected void txtInvoiceTotal_TextChanged(object sender, EventArgs e)
        {
            double points = 0;
            points = Convert.ToDouble(txtInvoiceTotal.Text) * (0.5 / 100);
            txtPoints.Text = points.ToString();
        }


        string cusid="";
        string mobile;
        protected void txtCustomerPhoneNo_TextChanged(object sender, EventArgs e)
        {
            string phone = txtCustomerPhoneNo.Text;
            string invnum = txtInvoiceNo.Text;

            // Call the method and get the invoice details
            InvoiceDetails details = cuscon.get_cusname(phone);

            if (details != null && details.CustomerName != "0")
            {
                //txtInvoiceAmount.Text = details.InvoiceAmount.ToString("N2"); // Format as 2-decimal currency
                txtCustomerName.Text = details.CustomerName;
                mobile = details.MobileNumber;
                cusid = details.CustomerId;

                
                txtCustomerName.ForeColor = System.Drawing.Color.Blue;

                txtInvoiceNo.Focus();
            }
            else
            {
                txtCustomerName.Text = "Invalid Mobile or not Registerd";
                txtCustomerName.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}