using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Point_Card_System.DAL;

namespace Point_Card_System.Pages
{
    public partial class CustomerRegistration1 : System.Web.UI.Page
    {
        CustomerController cuscon = new CustomerController();
        string branchcode = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Session["Username"] as string;
            string password = Session["Password"] as string;
            if (username != null)
            {
                get_branch_code(username, password);
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                get_customer_code();
                txtRegisteredDate.Text = DateTime.Now.ToString("yyyy-MM-dd"); 
                txtRegisteredBranch.Text = branchcode;
            }
        }


        void get_customer_code()
        {
            int oldCode = Int32.Parse(cuscon.Get_customer_code());
            txtCustomerID.Text = (oldCode + 1).ToString("0000");
        }

        void get_branch_code(string user, string password)
        {
            branchcode = cuscon.Get_customer_branchcode(user, password);
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            // Server-side validation as backup
            if (!ValidateServerSide())
            {
                return;
            }

            int id = string.IsNullOrEmpty(hfCustomerId.Value) ? 0 : Convert.ToInt32(hfCustomerId.Value);

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "Id", id },
                    { "Customer_Id", txtCustomerID.Text.Trim() },
                    { "Customer_Name", txtCustomerName.Text.Trim() },
                    { "Address1", txtAddress.Text.Trim() },
                    { "Address2", "" },
                    { "Mobile_Number", txtPhoneNo.Text.Trim() },
                    { "NIC", txtNIC.Text.Trim() },
                    { "Branch_ID", branchcode },
                    { "Available_Points", 0},
                    { "Created_Date", DateTime.Now },
                    { "Created_Time", DateTime.Now },
                    { "Active", '1' },
                    { "isVat", rbVatCustomer.Checked ? '1' : '0'},
                    { "card_numcer", txtCardNum.Text.Trim() }
                };

                if (id > 0)
                {
                    // Update logic here (if you have update functionality)
                    // cuscon.UpdateCustomer(parameters);
                    ShowSuccessMessage("Customer information updated successfully!");
                }
                else
                {
                    string customerName = txtCustomerName.Text.Trim();
                    string mobileNumber = txtPhoneNo.Text.Trim();

                    // Block registration if either name or phone exists
                    if (cuscon.IsCustomerExists(customerName, mobileNumber))
                    {
                        ShowErrorMessage("A customer with this name or phone number already exists.");
                        return;
                    }

                    cuscon.InsertCustomer(parameters);
                    ShowSuccessMessage("Customer registration successful");
                    ClearFormFields();
                    get_customer_code();
                }
                hfCustomerId.Value = string.Empty;
            }
            catch (Exception ex)
            {
                // Log the exception (implement proper logging in production)
                System.Diagnostics.Debug.WriteLine($"Error in customer registration: {ex.Message}");

                ShowErrorMessage("Customer registration failed");
            }
        }

        private void ClearFormFields()
        {
            txtCustomerName.Text = "";
            txtNIC.Text = "";
            txtPhoneNo.Text = "";
            txtAddress.Text = "";
            txtCardNum.Text = "";
            rbVatCustomer.Checked = false;
            rbNonVatCustomer.Checked = false;
            txtClammingPoints.Text = "0";
        }

        private bool ValidateServerSide()
        {
            bool isValid = true;
            string errorMessage = "";

            // Validate Customer Name
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                errorMessage += "Customer Name is required. ";
                isValid = false;
            }

            // Validate Phone Number
            if (string.IsNullOrWhiteSpace(txtPhoneNo.Text))
            {
                errorMessage += "Phone Number is required. ";
                isValid = false;
            }
            else if (!IsValidPhoneNumber(txtPhoneNo.Text.Trim()))
            {
                errorMessage += "Phone number must be 10 digits. ";
                isValid = false;
            }

            if (!isValid)
            {
                ShowValidationError(errorMessage.Trim());
            }

            return isValid;
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Check if phone number is exactly 10 digits
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^[0-9]{10}$");
        }

        // SweetAlert2 Helper Methods
        private void ShowSuccessMessage(string message)
        {
            string script = $@"
                Swal.fire({{
                    title: 'Success!',
                    text: '{message}',
                    icon: 'success',
                    confirmButtonText: 'OK',
                    confirmButtonColor: '#28a745',
                    timer: 3000,
                    timerProgressBar: true
                }});";

            ClientScript.RegisterStartupScript(this.GetType(), "showSuccess", script, true);
        }

        private void ShowErrorMessage(string message)
        {
            string script = $@"
                Swal.fire({{
                    title: 'Error!',
                    text: '{message}',
                    icon: 'error',
                    confirmButtonText: 'OK',
                    confirmButtonColor: '#dc3545'
                }});";
           
            ClientScript.RegisterStartupScript(this.GetType(), "showError", script, true);
        }

        private void ShowValidationError(string message)
        {
            string script = $@"
                Swal.fire({{
                    title: 'Validation Error!',
                    text: '{message}',
                    icon: 'warning',
                    confirmButtonText: 'OK',
                    confirmButtonColor: '#ffc107'
                }});";

            ClientScript.RegisterStartupScript(this.GetType(), "showValidationError", script, true);
        }

        // Text changed event handlers for navigation
        protected void txtAddress_TextChanged(object sender, EventArgs e)
        {
            btn_save.Enabled = true;
            btn_save.Focus();
        }

        protected void txtPhoneNo_TextChanged(object sender, EventArgs e)
        {
            txtAddress.Focus();
        }

        protected void txtNIC_TextChanged(object sender, EventArgs e)
        {
            txtPhoneNo.Focus();
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            txtNIC.Focus();
        }

        protected void txtCardNum_TextChanged(object sender, EventArgs e)
        {
            btn_save.Enabled = true;
            btn_save.Focus();
        }
    }
}