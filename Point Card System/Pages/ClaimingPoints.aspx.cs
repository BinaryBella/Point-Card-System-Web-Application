//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Security.Policy;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using Point_Card_System.DAL;
//using static Point_Card_System.DAL.CustomerController;

//namespace Point_Card_System.Pages
//{
//    public partial class ClaimingPoints : System.Web.UI.Page
//    {
//        CustomerController cuscon = new CustomerController();
//        string branchcode = "";
//        string userid = "";

//        protected void Page_Load(object sender, EventArgs e)
//        {
//            btnSave.Enabled = false;
//            string username = Session["Username"] as string;
//            string password = Session["Password"] as string;
//            if (username != null)
//            {
//                // If session exists, display the username or use it for some purpose
//                get_branch_code(username, password);
//            }
//            else
//            {
//                // If session does not exist (user is not logged in), redirect to login page
//                Response.Redirect("Login.aspx");
//            }

//            // Enable AutoPostBack for phone number textbox
//            txtPhoneNo.AutoPostBack = true;
//        }

//        void get_branch_code(string user, string password)
//        {
//            branchcode = cuscon.Get_customer_branchcode(user, password);
//            userid = cuscon.Get_userid(user, password);
//        }

//        string cusid = "";
//        string mobile;

//        protected void txtPhoneNo_TextChanged(object sender, EventArgs e)
//        {
//            string phone = txtPhoneNo.Text.Trim();

//            // Only proceed if phone number has minimum required length
//            if (phone.Length < 10)
//            {
//                // Clear the fields if phone number is too short
//                txtCustomerName.Text = "";
//                txtAvailablePoints.Text = "";
//                return;
//            }

//            // Call the method and get the invoice details
//            InvoiceDetails details = cuscon.get_cusPoints(phone);

//            if (details != null && details.CustomerName != "0")
//            {
//                txtCustomerName.Text = details.CustomerName;
//                txtAvailablePoints.Text = (details.totalpoints).ToString();
//                mobile = details.MobileNumber;
//                cusid = details.CustomerId;

//                txtCustomerName.ForeColor = System.Drawing.Color.Blue;

//                // Register script to focus on invoice field after successful validation
//                string focusScript = "focusInvoiceField();";
//                ClientScript.RegisterStartupScript(this.GetType(), "FocusInvoice", focusScript, true);
//            }
//            else
//            {
//                // Clear the fields
//                txtCustomerName.Text = "";
//                txtAvailablePoints.Text = "";

//                // Show SweetAlert2 error message
//                string script = @"
//                    Swal.fire({
//                        icon: 'error',
//                        title: 'Invalid Mobile Number',
//                        text: 'Invalid mobile number or not registered',
//                        confirmButtonText: 'OK',
//                        confirmButtonColor: '#dc3545'
//                    }).then(function() {
//                        // Focus back to phone number field
//                        document.getElementById('" + txtPhoneNo.ClientID + @"').focus();
//                    });";

//                ClientScript.RegisterStartupScript(this.GetType(), "InvalidMobileAlert", script, true);
//            }
//        }

//        protected void txtClaimingPoints_TextChanged(object sender, EventArgs e)
//        {
//            if (!string.IsNullOrEmpty(txtAvailablePoints.Text) && !string.IsNullOrEmpty(txtClaimingPoints.Text))
//            {
//                double availablePoints = 0;
//                double claimingPoints = 0;

//                if (double.TryParse(txtAvailablePoints.Text, out availablePoints) &&
//                    double.TryParse(txtClaimingPoints.Text, out claimingPoints))
//                {
//                    if (availablePoints >= 1000 && claimingPoints <= availablePoints && claimingPoints > 0)
//                    {
//                        btnSave.Enabled = true;

//                        // Register script to focus on save button
//                        string focusScript = "setTimeout(function() { document.getElementById('" + btnSave.ClientID + "').focus(); }, 100);";
//                        ClientScript.RegisterStartupScript(this.GetType(), "FocusSave", focusScript, true);
//                    }
//                    else
//                    {
//                        btnSave.Enabled = false;

//                        // Show SweetAlert2 warning message for insufficient points
//                        string script = @"
//                            Swal.fire({
//                                icon: 'warning',
//                                title: 'Insufficient Points',
//                                text: 'Sorry! You cannot claim this amount. Minimum points should be 1000 to redeem!',
//                                confirmButtonText: 'OK',
//                                confirmButtonColor: '#ffc107'
//                            });";

//                        ClientScript.RegisterStartupScript(this.GetType(), "InsufficientPointsAlert", script, true);
//                    }
//                }
//                else
//                {
//                    btnSave.Enabled = false;
//                }
//            }
//        }

//        protected void btnSave_Click(object sender, EventArgs e)
//        {
//            string phone = txtPhoneNo.Text;
//            InvoiceDetails details = cuscon.get_cusPoints(phone);
//            string name = txtCustomerName.Text;
//            string points = txtClaimingPoints.Text;
//            string branchid = branchcode;
//            cusid = details.CustomerId;

//            try
//            {
//                var parameters = new Dictionary<string, object>
//                {
//                    { "date", DateTime.Now },
//                    { "branch_id", branchcode},
//                    { "user_id", userid},
//                    { "points", double.Parse(points)},
//                    { "cus_phone", phone },
//                    { "Description", "Points Claims for" + " "+txtInvoiceNum.Text},
//                    { "cus_id", cusid }
//                };

//                cuscon.Insert_Claimed_Points(parameters);

//                // Show success message with SweetAlert2
//                string script = @"
//                    Swal.fire({
//                        icon: 'success',
//                        title: 'Points Claimed Successfully!',
//                        text: 'Points have been claimed for " + name + @"',
//                        confirmButtonText: 'OK',
//                        confirmButtonColor: '#28a745'
//                    }).then(function() {
//                        // Clear form or redirect as needed
//                        window.location.href = window.location.href;
//                    });";

//                ClientScript.RegisterStartupScript(this.GetType(), "SuccessAlert", script, true);
//            }
//            catch (Exception ex)
//            {
//                // Show error message with SweetAlert2
//                string script = @"
//                    Swal.fire({
//                        icon: 'error',
//                        title: 'Error',
//                        text: 'An error occurred while processing your request. Please try again.',
//                        confirmButtonText: 'OK',
//                        confirmButtonColor: '#dc3545'
//                    });";

//                ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", script, true);
//            }
//        }

//        protected void txtInvoiceNum_TextChanged(object sender, EventArgs e)
//        {
//            // Register script to focus on claiming points field
//            string focusScript = "setTimeout(function() { document.getElementById('" + txtClaimingPoints.ClientID + "').focus(); }, 100);";
//            ClientScript.RegisterStartupScript(this.GetType(), "FocusClaimingPoints", focusScript, true);
//        }
//    }
//}

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
            string phone = txtPhoneNo.Text.Trim();

            // Only proceed if phone number has minimum required length
            if (phone.Length < 10)
            {
                // Clear the fields if phone number is too short
                txtCustomerName.Text = "";
                txtAvailablePoints.Text = "";
                return;
            }

            // Call the method and get the invoice details
            InvoiceDetails details = cuscon.get_cusPoints(phone);

            if (details != null && details.CustomerName != "0")
            {
                txtCustomerName.Text = details.CustomerName;
                txtAvailablePoints.Text = (details.totalpoints).ToString();
                mobile = details.MobileNumber;
                cusid = details.CustomerId;

                txtCustomerName.ForeColor = System.Drawing.Color.Blue;

                // Register script to focus on invoice field and check form completion
                string focusScript = @"
                    focusInvoiceField();
                    setTimeout(checkFormCompletion, 200);
                ";
                ClientScript.RegisterStartupScript(this.GetType(), "FocusInvoice", focusScript, true);
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
                    }).then(function() {
                        // Focus back to phone number field and check form completion
                        document.getElementById('" + txtPhoneNo.ClientID + @"').focus();
                        checkFormCompletion();
                    });";

                ClientScript.RegisterStartupScript(this.GetType(), "InvalidMobileAlert", script, true);
            }
        }

        protected void txtClaimingPoints_TextChanged(object sender, EventArgs e)
        {
            // Register script to check form completion after claiming points change
            string checkScript = "setTimeout(checkFormCompletion, 100);";
            ClientScript.RegisterStartupScript(this.GetType(), "CheckFormCompletion", checkScript, true);

            if (!string.IsNullOrEmpty(txtAvailablePoints.Text) && !string.IsNullOrEmpty(txtClaimingPoints.Text))
            {
                double availablePoints = 0;
                double claimingPoints = 0;

                if (double.TryParse(txtAvailablePoints.Text, out availablePoints) &&
                    double.TryParse(txtClaimingPoints.Text, out claimingPoints))
                {
                    if (availablePoints < 1000)
                    {
                        // Show warning for minimum points requirement
                        string script = @"
                            Swal.fire({
                                icon: 'warning',
                                title: 'Insufficient Points',
                                text: 'Sorry! Minimum points should be 1000 to redeem!',
                                confirmButtonText: 'OK',
                                confirmButtonColor: '#ffc107'
                            });";

                        ClientScript.RegisterStartupScript(this.GetType(), "MinimumPointsAlert", script, true);
                    }
                    else if (claimingPoints > availablePoints)
                    {
                        // Show warning for claiming more than available
                        string script = @"
                            Swal.fire({
                                icon: 'warning',
                                title: 'Invalid Claim Amount',
                                text: 'You cannot claim more points than available!',
                                confirmButtonText: 'OK',
                                confirmButtonColor: '#ffc107'
                            });";

                        ClientScript.RegisterStartupScript(this.GetType(), "ExcessivePointsAlert", script, true);
                    }
                    else if (claimingPoints <= 0)
                    {
                        // Show warning for invalid points
                        string script = @"
                            Swal.fire({
                                icon: 'warning',
                                title: 'Invalid Points',
                                text: 'Please enter a valid positive number for claiming points!',
                                confirmButtonText: 'OK',
                                confirmButtonColor: '#ffc107'
                            });";

                        ClientScript.RegisterStartupScript(this.GetType(), "InvalidPointsAlert", script, true);
                    }
                }
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
            // Register script to focus on claiming points field and check form completion
            string script = @"
                setTimeout(function() { 
                    document.getElementById('" + txtClaimingPoints.ClientID + @"').focus(); 
                    checkFormCompletion();
                }, 100);";
            ClientScript.RegisterStartupScript(this.GetType(), "FocusClaimingPoints", script, true);
        }
    }
}