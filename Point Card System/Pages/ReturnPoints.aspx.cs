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
                // Clear the fields and show SweetAlert2 error
                txtCustomerName.Text = string.Empty;
                txtInvoiceAmount.Text = string.Empty;

                // Show SweetAlert2 error message using client script
                string script = @"
                    <script type='text/javascript'>
                        setTimeout(function() {
                            showInvalidInvoiceAlert();
                        }, 100);
                    </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "InvalidInvoiceAlert", script);
            }
        }

        protected void txtReturnAmount_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txtInvoiceAmount.Text) < Convert.ToDouble(txtReturnAmount.Text))
            {
                // Show SweetAlert2 error message for invalid return amount
                string script = @"
                    <script type='text/javascript'>
                        setTimeout(function() {
                            Swal.fire({
                                icon: 'error',
                                title: 'Invalid Amount',
                                text: 'Return amount cannot be greater than invoice amount.',
                                confirmButtonText: 'OK',
                                confirmButtonColor: '#dc3545'
                            });
                        }, 100);
                    </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "InvalidAmountAlert", script);
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

                // Show success SweetAlert2 message
                string successScript = @"
                    <script type='text/javascript'>
                        setTimeout(function() {
                            showSuccessAlert('Customer points returned successfully.');
                        }, 100);
                    </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "SuccessAlert", successScript);

                // Clear form fields after successful save
                ClearFormFields();
            }
            catch (Exception ex)
            {
                // Show error SweetAlert2 message
                string errorScript = @"
                    <script type='text/javascript'>
                        setTimeout(function() {
                            showErrorAlert('Failed to add customer return.');
                        }, 100);
                    </script>";
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorAlert", errorScript);

                // Log the exception (you might want to add proper logging here)
                // System.Diagnostics.Debug.WriteLine("Error in btnSave_Click: " + ex.Message);
            }
        }

        private void ClearFormFields()
        {
            txtInvoiceNo.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtInvoiceAmount.Text = string.Empty;
            txtReturnAmount.Text = string.Empty;
            txtAvailablePoints.Text = string.Empty;
            txtReturnInvNum.Text = string.Empty;
            btnSave.Enabled = false;
        }
    }
}