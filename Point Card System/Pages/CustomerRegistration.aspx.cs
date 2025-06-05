using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Point_Card_System.DAL;
using System.Data;

namespace Point_Card_System.Pages
{
    public partial class CustomerRegistration1 : System.Web.UI.Page
    {
        CustomerController cuscon = new CustomerController();
        string branchcode = "";
        int userLevel = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null || Session["Password"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            string username = Session["Username"].ToString();
            string password = Session["Password"].ToString();

            if (!IsPostBack)
            {
                CustomerController controller = new CustomerController();

                // Set customer code and date
                get_customer_code();
                txtRegisteredDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                // Get values from session (already set during authentication)
                string branchId = Session["branch_id"]?.ToString() ?? "";
                userLevel = Session["user_level"] != null ? Convert.ToInt32(Session["user_level"]) : 0;

                // If session values are missing, fetch from database as fallback
                if (string.IsNullOrEmpty(branchId) || userLevel == 0)
                {
                    branchId = controller.Get_customer_branchcode(username, password);
                    userLevel = controller.Get_user_level(username, password);

                    // Update session with fetched values
                    Session["branch_id"] = branchId;
                    Session["user_level"] = userLevel;
                }

                // Setup UI
                SetupBranchField(branchId, userLevel);
            }
            else
            {
                // On postback, retrieve from session
                if (Session["user_level"] != null)
                {
                    userLevel = Convert.ToInt32(Session["user_level"]);
                }

                if (Session["branch_id"] != null)
                {
                    branchcode = Session["branch_id"].ToString();
                }
            }
        }
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (Session["Username"] == null || Session["Password"] == null)
        //    {
        //        Response.Redirect("Login.aspx");
        //        return;
        //    }

        //    string username = Session["Username"].ToString();
        //    string password = Session["Password"].ToString();

        //    if (!IsPostBack)
        //    {
        //        CustomerController controller = new CustomerController();

        //        // Set customer code and date
        //        get_customer_code();
        //        txtRegisteredDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

        //        // Fetch branch and level
        //        string branchId = controller.Get_customer_branchcode(username, password);
        //        userLevel = controller.Get_user_level(username, password);

        //        // Store in session for persistence across postbacks
        //        Session["branch_id"] = branchId;
        //        Session["user_level"] = userLevel;

        //        // Setup UI
        //        SetupBranchField(branchId, userLevel);
        //    }
        //    else
        //    {
        //        // On postback, retrieve from session
        //        if (Session["user_level"] != null)
        //        {
        //            userLevel = Convert.ToInt32(Session["user_level"]);
        //        }

        //        if (Session["branch_id"] != null)
        //        {
        //            branchcode = Session["branch_id"].ToString();
        //        }
        //    }
        //}

        private void SetupBranchField(string branchId, int userLevel)
        {
            CustomerController controller = new CustomerController();

            if (userLevel == 1)
            {
                ddlRegisteredBranch.DataSource = controller.GetAllBranches();
                ddlRegisteredBranch.DataTextField = "branch_name";
                ddlRegisteredBranch.DataValueField = "branch_id";
                ddlRegisteredBranch.DataBind();

                ddlRegisteredBranch.Visible = true;
                txtRegisteredBranch.Visible = false;
            }
            else
            {
                txtRegisteredBranch.Text = controller.GetBranchName(branchId);
                txtRegisteredBranch.ReadOnly = true;
                txtRegisteredBranch.Visible = true;
                ddlRegisteredBranch.Visible = false;
            }
        }

        void LoadBranches()
        {
            try
            {
                DataTable branches = cuscon.GetAllBranches();
                ddlRegisteredBranch.DataSource = branches;
                ddlRegisteredBranch.DataTextField = "branch_name";
                ddlRegisteredBranch.DataValueField = "branch_id";
                ddlRegisteredBranch.DataBind();

                // Add default selection
                ddlRegisteredBranch.Items.Insert(0, new ListItem("Select Branch", ""));

                // If user has a specific branch, select it by default
                if (!string.IsNullOrEmpty(branchcode))
                {
                    ListItem item = ddlRegisteredBranch.Items.FindByValue(branchcode);
                    if (item != null)
                    {
                        ddlRegisteredBranch.SelectedValue = branchcode;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading branches: {ex.Message}");
                ShowErrorMessage("Error loading branch data");
            }
        }

        string GetBranchName(string branchId)
        {
            try
            {
                return cuscon.GetBranchName(branchId);
            }
            catch
            {
                return branchId; // Fallback to branch code if name not found
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

        void get_user_level(string user, string password)
        {
            userLevel = cuscon.Get_user_level(user, password);
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            // Server-side validation as backup
            if (!ValidateServerSide())
            {
                return;
            }

            int id = string.IsNullOrEmpty(hfCustomerId.Value) ? 0 : Convert.ToInt32(hfCustomerId.Value);

            // Get user level from session (more reliable than field)
            int currentUserLevel = 0;
            if (Session["user_level"] != null)
            {
                currentUserLevel = Convert.ToInt32(Session["user_level"]);
            }

            // Get the selected branch based on user level
            string selectedBranch = "";
            if (currentUserLevel == 1)
            {
                selectedBranch = ddlRegisteredBranch.SelectedValue;
                if (string.IsNullOrEmpty(selectedBranch))
                {
                    ShowErrorMessage("Please select a branch");
                    return;
                }
            }
            else
            {
                // For non-level 1 users, get branch from session
                selectedBranch = Session["branch_id"]?.ToString() ?? branchcode;
            }

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
            { "Branch_ID", selectedBranch },
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

            // Reset branch field based on user level
            if (userLevel == 1)
            {
                ddlRegisteredBranch.SelectedIndex = 0;
            }
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

            // Validate Branch Selection for user level 1
            if (userLevel == 1 && string.IsNullOrEmpty(ddlRegisteredBranch.SelectedValue))
            {
                errorMessage += "Please select a branch. ";
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