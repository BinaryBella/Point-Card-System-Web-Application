using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Point_Card_System.DAL;
using static Point_Card_System.DAL.CustomerController;

namespace Point_Card_System.Pages
{
    public partial class ClaimingPoints : System.Web.UI.Page
    {
        CustomerController cuscon = new CustomerController();
        string branchcode = "";
        string userid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
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

        string cusid = "";
        string mobile;
        protected void txtPhoneNo_TextChanged(object sender, EventArgs e)
        {
            string phone = txtPhoneNo.Text;


            // Call the method and get the invoice details
            InvoiceDetails details = cuscon.get_cusPoints(phone);

            if (details != null && details.CustomerName != "0")
            {
                //txtInvoiceAmount.Text = details.InvoiceAmount.ToString("N2"); // Format as 2-decimal currency
                txtCustomerName.Text = details.CustomerName;
                txtAvailablePoints.Text = (details.totalpoints).ToString();
                mobile = details.MobileNumber;
                cusid = details.CustomerId;


                txtCustomerName.ForeColor = System.Drawing.Color.Blue;

                txtInvoiceNum.Focus();
            }
            else
            {
                txtCustomerName.Text = "Invalid Mobile or not Registerd";
                txtCustomerName.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void txtClaimingPoints_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txtAvailablePoints.Text) >= 1000 && Convert.ToDouble(txtClaimingPoints.Text) <= Convert.ToDouble(txtAvailablePoints.Text))
            {
                btnSave.Enabled = true;
                btnSave.Focus();
            }
            else
            {
                string message = "Sorry You cannot claim this ammount! Minimum Points Should be 1000 to Redeam!";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{message}');", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string phone = txtPhoneNo.Text;
            InvoiceDetails details = cuscon.get_cusPoints(phone);
            string name = txtCustomerName.Text;

            string points = txtClaimingPoints.Text;
            string branchid = branchcode;
            cusid = details.CustomerId;

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "date", DateTime.Now },
                    { "branch_id", branchcode},
                    { "user_id", userid},
                    { "points", double.Parse(points)},
                    { "cus_phone", phone },
                    { "Description", "Points Claims for" + " "+txtInvoiceNum.Text},
                    { "cus_id", cusid }
                };
                cuscon.Insert_Claimed_Points(parameters);
                Response.Write("Points Claimed: " + name);

                //hfCustomerId.Value = string.Empty;
                //textbox_Name.Text = "";

                //get_customer_code();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void txtInvoiceNum_TextChanged(object sender, EventArgs e)
        {
            txtClaimingPoints.Focus();
        }
    }
}