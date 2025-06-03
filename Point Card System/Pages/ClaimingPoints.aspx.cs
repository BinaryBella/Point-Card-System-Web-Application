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
                txtCustomerName.Text = details.CustomerName;
                txtAvailablePoints.Text = (details.totalpoints).ToString();
                mobile = details.MobileNumber;
                cusid = details.CustomerId;

                txtCustomerName.ForeColor = System.Drawing.Color.Blue;
                txtInvoiceNum.Focus();
            }
            else
            {
                // Clear the fields
                txtCustomerName.Text = "";
                txtAvailablePoints.Text = "";

                // Show SweetAlert2 error message
                string script = @"
                    Swal.fire({
                        icon: 'error',
                        title: 'Invalid Mobile Number',
                        text: 'Invalid mobile number or not registered',
                        confirmButtonText: 'OK',
                        confirmButtonColor: '#dc3545'
                    });";

                ClientScript.RegisterStartupScript(this.GetType(), "InvalidMobileAlert", script, true);
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
                // Show SweetAlert2 warning message for insufficient points
                string script = @"
                    Swal.fire({
                        icon: 'warning',
                        title: 'Insufficient Points',
                        text: 'Sorry! You cannot claim this amount. Minimum points should be 1000 to redeem!',
                        confirmButtonText: 'OK',
                        confirmButtonColor: '#ffc107'
                    });";

                ClientScript.RegisterStartupScript(this.GetType(), "InsufficientPointsAlert", script, true);
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

                // Show success message with SweetAlert2
                string script = @"
                    Swal.fire({
                        icon: 'success',
                        title: 'Points Claimed Successfully!',
                        text: 'Points have been claimed for " + name + @"',
                        confirmButtonText: 'OK',
                        confirmButtonColor: '#28a745'
                    }).then(function() {
                        // Clear form or redirect as needed
                        window.location.href = window.location.href;
                    });";

                ClientScript.RegisterStartupScript(this.GetType(), "SuccessAlert", script, true);
            }
            catch (Exception ex)
            {
                // Show error message with SweetAlert2
                string script = @"
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'An error occurred while processing your request. Please try again.',
                        confirmButtonText: 'OK',
                        confirmButtonColor: '#dc3545'
                    });";

                ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
            }
        }

        protected void txtInvoiceNum_TextChanged(object sender, EventArgs e)
        {
            txtClaimingPoints.Focus();
        }
    }
}