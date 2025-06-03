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
            //btn_save.Enabled = false;
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
            if (!IsPostBack)
            {
                get_customer_code();
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
            int id = string.IsNullOrEmpty(hfCustomerId.Value) ? 0 : Convert.ToInt32(hfCustomerId.Value);
            int cusType = 1;
            try
            {
                var parameters = new Dictionary<string, object>
        {
            { "Id", id },
            { "Customer_Id", txtCustomerID.Text },
            { "Customer_Name", txtCustomerName.Text },
            { "Address1", txtAddress.Text },
            { "Address2", "" },
            { "Mobile_Number", txtPhoneNo.Text },
            { "NIC", txtNIC.Text },
            { "Branch_ID", branchcode },
            { "Available_Points", 0},
            { "Created_Date", DateTime.Now },
            { "Created_Time", DateTime.Now },
            { "Active", '1' },
            { "isVat", rbVatCustomer.Checked ? '1' : '0'},
            { "card_numcer", txtCardNum.Text }
        };

                if (id > 0)
                {
                    // Update logic here
                    // cuscon.UpdateCustomer(parameters);
                    // Show success message for update
                    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess",
                        "showSuccessMessage('Customer information updated successfully!');", true);
                }
                else
                {
                    cuscon.InsertCustomer(parameters);
                    // Show success message for insert
                    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess",
                        "showSuccessMessage('Customer registered successfully!');", true);
                }

                hfCustomerId.Value = string.Empty;

                // Clear form fields after successful save
                ClearFormFields();

                get_customer_code();
            }
            catch (Exception ex)
            {
                // Log the exception (you should implement proper logging)
                // Logger.LogError(ex, "Error saving customer");

                // Show error message
                ClientScript.RegisterStartupScript(this.GetType(), "showError",
                    $"showErrorMessage('Error saving customer: {ex.Message}');", true);
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

        // Updated server-side validation - removed NIC, Address, and Customer Type validations
        private bool ValidateServerSide()
        {
            bool isValid = true;
            string errorMessage = "";

            // Validate Customer Name - only check if not empty
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                errorMessage += "Customer Name is required. ";
                isValid = false;
            }

            // Validate Phone Number
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhoneNo.Text.Trim(), @"^0[0-9]{9}$"))
            {
                errorMessage += "Phone number must be 10 digits starting with 0. ";
                isValid = false;
            }

            if (!isValid)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "showValidationError",
                    $"showErrorMessage('{errorMessage.Trim()}');", true);
            }

            return isValid;
        }

        // Modified btn_save_Click with server-side validation
        protected void btn_save_Click_WithValidation(object sender, EventArgs e)
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
            { "Customer_Id", txtCustomerID.Text },
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
                    // Update logic here
                    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess",
                        "showSuccessMessage('Customer information updated successfully!');", true);
                }
                else
                {
                    cuscon.InsertCustomer(parameters);
                    ClientScript.RegisterStartupScript(this.GetType(), "showSuccess",
                        "showSuccessMessage('Customer registered successfully!');", true);
                }

                hfCustomerId.Value = string.Empty;
                ClearFormFields();
                get_customer_code();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "showError",
                    $"showErrorMessage('Error saving customer: Please try again.');", true);
            }
        }

        //protected void btn_save_Click(object sender, EventArgs e)
        //{
        //    int id = string.IsNullOrEmpty(hfCustomerId.Value) ? 0 : Convert.ToInt32(hfCustomerId.Value);
        //    int cusType = 1;
        //    try
        //    {
        //        var parameters = new Dictionary<string, object>
        //        {
        //            { "Id", id },
        //            { "Customer_Id", txtCustomerID.Text },
        //            { "Customer_Name", txtCustomerName.Text },
        //            { "Address1", txtAddress.Text },
        //            { "Address2", "" },
        //            { "Mobile_Number", txtPhoneNo.Text },
        //            { "NIC", txtNIC.Text },
        //            { "Branch_ID", branchcode },
        //            { "Available_Points", 0},
        //            { "Created_Date", DateTime.Now },
        //            { "Created_Time", DateTime.Now },
        //            { "Active", '1' },
        //            { "isVat", '0'},
        //            { "card_numcer", txtCardNum.Text }
        //        };

        //        if (id > 0)
        //        {

        //        }
        //        else
        //        {
        //            cuscon.InsertCustomer(parameters);
        //        }

        //        hfCustomerId.Value = string.Empty;
        //        //textbox_Name.Text = "";

        //        get_customer_code();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

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