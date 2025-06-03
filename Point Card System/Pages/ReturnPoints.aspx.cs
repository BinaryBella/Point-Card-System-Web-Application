using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Point_Card_System.DAL;
using static Point_Card_System.DAL.CustomerController;

namespace Point_Card_System.Pages
{
    public partial class ReturnPoints1 : System.Web.UI.Page
    {
        CustomerController cuscon = new CustomerController();
        string branchcode = "";
        string userid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Session["Username"] as string;
            string password = Session["Password"] as string;
            btnSave.Enabled = false;
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

        string cusid;
        string mobile;
        protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
        {
            string invnum = txtInvoiceNo.Text;

            // Call the method and get the invoice details
            InvoiceDetails details = cuscon.GetInvoiceDetails(invnum, branchcode);

            if (details != null && details.InvoiceAmount > 0)
            {
                txtInvoiceAmount.Text = details.InvoiceAmount.ToString("N2"); // Format as 2-decimal currency
                txtCustomerName.Text = details.CustomerName;
                mobile = details.MobileNumber;
                cusid = details.CustomerId;

                txtInvoiceAmount.ForeColor = System.Drawing.Color.Blue;
                txtCustomerName.ForeColor = System.Drawing.Color.Blue;

                txtReturnAmount.Focus();
            }
            else
            {
                txtCustomerName.Text = "Invalid invoice number or 0 amount invoice";
                txtCustomerName.ForeColor = System.Drawing.Color.Red;

                txtInvoiceAmount.Text = string.Empty;
                //txtCustomerName.Text = string.Empty;
            }
        }

        protected void txtReturnAmount_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txtInvoiceAmount.Text) < Convert.ToDouble(txtReturnAmount.Text))
            {
                string message = "Invalid invoice amount";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{message}');", true);
            }
            else
            {
                double points = 0;
                points = Convert.ToDouble(txtReturnAmount.Text) * (0.5 / 100);
                txtAvailablePoints.Text = points.ToString();
                txtReturnInvNum.Focus();
               
            }
        }

        protected void txtReturnInvNum_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            btnSave.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Example: Save entered data
            string invnum = txtInvoiceNo.Text;
            InvoiceDetails details = cuscon.GetInvoiceDetails(invnum, branchcode);
            string return_inv_num = txtReturnInvNum.Text;
            string name = txtCustomerName.Text;
            string invoiceNo = txtInvoiceNo.Text;
            string return_amt = txtReturnAmount.Text;
            string points = txtAvailablePoints.Text;
            string branchid = branchcode;
            mobile = details.MobileNumber;
            cusid = details.CustomerId;

            try
            {
                var parameters = new Dictionary<string, object>
                {

                    { "invoice_no", return_inv_num },
                    { "invoice_amount", double.Parse(return_amt) },
                    { "date", DateTime.Now },
                    { "branch_id", branchcode},
                    { "user_id", userid},
                    { "points", double.Parse(points)},
                    { "cus_phone", mobile },
                    { "Description", "Invoice Return" + " "+invoiceNo},
                    { "cus_id", cusid }
                };
                cuscon.Insert_return_PointsDetails(parameters);
                Response.Write("Points Returned: " + name);

                //hfCustomerId.Value = string.Empty;
                //textbox_Name.Text = "";

                //get_customer_code();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}