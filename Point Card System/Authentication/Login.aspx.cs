//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Web.UI;

//namespace Point_Card_System.Authentication
//{
//    public partial class Login : System.Web.UI.Page
//    {
//        private readonly string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;

//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (!IsPostBack)
//            {
//                // Clear any previous error messages
//                lblLoginError.Visible = false;

//                // Set focus to username field
//                TextBox_username.Focus();
//            }
//        }

//        protected void Button1_Click(object sender, EventArgs e)
//        {
//            // Clear previous error messages
//            lblLoginError.Visible = false;

//            try
//            {
//                // Server-side validation
//                if (!ValidateInput())
//                {
//                    return;
//                }

//                string username = TextBox_username.Text.Trim();
//                string password = txtPassword.Text;

//                // Authenticate user against database
//                if (Authenticate(username, password))
//                {
//                    // Store the username in the session
//                    Session["Username"] = username;
//                    Session["Password"] = password; // Note: Consider not storing password in session for security

//                    // Redirect to another page (e.g., Dashboard)
//                    Response.Redirect("~/Default.aspx");
//                }
//                else
//                {
//                    ShowError("Invalid username or password. Please check your credentials and try again.");

//                    // Clear password field for security
//                    txtPassword.Text = "";

//                    // Focus back to username if empty, otherwise password
//                    if (string.IsNullOrWhiteSpace(TextBox_username.Text))
//                        TextBox_username.Focus();
//                    else
//                        txtPassword.Focus();
//                }
//            }
//            catch (SqlException sqlEx)
//            {
//                // Log the SQL exception (you should implement proper logging)
//                ShowError("Database connection error. Please try again later.");

//                // For debugging purposes - remove in production
//                System.Diagnostics.Debug.WriteLine($"SQL Error: {sqlEx.Message}");
//            }
//            catch (Exception ex)
//            {
//                // Log the general exception (you should implement proper logging)
//                ShowError("An unexpected error occurred. Please try again later.");

//                // For debugging purposes - remove in production
//                System.Diagnostics.Debug.WriteLine($"General Error: {ex.Message}");
//            }
//        }

//        private bool ValidateInput()
//        {
//            // Only validate that password is provided
//            if (string.IsNullOrWhiteSpace(txtPassword.Text))
//            {
//                ShowError("Password is required.");
//                txtPassword.Focus();
//                return false;
//            }

//            return true;
//        }

//        // Removed complex validation methods - keeping it simple

//        private void ShowError(string message)
//        {
//            lblLoginError.Text = message;
//            lblLoginError.Visible = true;
//        }

//        private bool Authenticate(string username, string password)
//        {
//            try
//            {
//                using (SqlConnection con = new SqlConnection(connectionString))
//                {
//                    using (SqlCommand cmd = new SqlCommand("SP_Master_User_Authenticate", con))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.CommandTimeout = 30; // Set timeout

//                        // Check if you need to hash the password or use plain text
//                        // Based on your original code, it seems you're using plain text in DB
//                        cmd.Parameters.AddWithValue("@Username", username);
//                        cmd.Parameters.AddWithValue("@Password", password); // Using plain text as per original code

//                        con.Open();
//                        object result = cmd.ExecuteScalar();

//                        if (result != null && result != DBNull.Value)
//                        {
//                            int codereturn = Convert.ToInt32(result);
//                            return codereturn == 1;
//                        }

//                        return false;
//                    }
//                }
//            }
//            catch (SqlException)
//            {
//                // Re-throw to be handled by calling method
//                throw;
//            }
//            catch (Exception)
//            {
//                // Re-throw to be handled by calling method
//                throw;
//            }
//        }

//        // Keep SHA256 hash method in case you need it later
//        private string ComputeSha256Hash(string rawData)
//        {
//            try
//            {
//                using (SHA256 sha256Hash = SHA256.Create())
//                {
//                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
//                    StringBuilder builder = new StringBuilder();
//                    foreach (byte b in bytes)
//                    {
//                        builder.Append(b.ToString("x2")); // Convert to hexadecimal
//                    }
//                    return builder.ToString();
//                }
//            }
//            catch (Exception)
//            {
//                // Handle hashing errors
//                throw new Exception("Error occurred while processing password.");
//            }
//        }

//        // Optional: Method to sanitize input (prevent SQL injection - though you're using parameters which is good)
//        private string SanitizeInput(string input)
//        {
//            if (string.IsNullOrWhiteSpace(input))
//                return string.Empty;

//            return input.Trim()
//                       .Replace("'", "''")  // Escape single quotes
//                       .Replace("--", "")   // Remove SQL comments
//                       .Replace("/*", "")   // Remove SQL comments
//                       .Replace("*/", "")   // Remove SQL comments
//                       .Replace("xp_", "")  // Remove extended stored procedure calls
//                       .Replace("sp_", "");  // Remove stored procedure calls
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Point_Card_System.Authentication
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any previous error messages
                lblLoginError.Visible = false;

                // Set focus to username field
                txtUsername.Focus();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Clear previous error messages
            lblLoginError.Visible = false;

            try
            {
                // Server-side validation
                if (!ValidateInput())
                {
                    return;
                }

                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;

                // Authenticate user against database
                if (Authenticate(username, password))
                {
                    // Store the username in the session
                    Session["Username"] = username;
                    // Note: Don't store password in session for security reasons

                    // Redirect to another page (e.g., Dashboard)
                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    ShowError("Invalid username or password. Please check your credentials and try again.");

                    // Clear password field for security
                    txtPassword.Text = "";

                    // Focus back to username if empty, otherwise password
                    if (string.IsNullOrWhiteSpace(txtUsername.Text))
                        txtUsername.Focus();
                    else
                        txtPassword.Focus();
                }
            }
            catch (SqlException sqlEx)
            {
                // Log the SQL exception (you should implement proper logging)
                ShowError("Database connection error. Please try again later.");

                // For debugging purposes - remove in production
                System.Diagnostics.Debug.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                // Log the general exception (you should implement proper logging)
                ShowError("An unexpected error occurred. Please try again later.");

                // For debugging purposes - remove in production
                System.Diagnostics.Debug.WriteLine($"General Error: {ex.Message}");
            }
        }

        private bool ValidateInput()
        {
            bool isValid = true;

            // Validate username
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowError("Username is required.");
                txtUsername.Focus();
                return false;
            }

            // Validate password
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowError("Password is required.");
                txtPassword.Focus();
                return false;
            }

            return isValid;
        }

        private void ShowError(string message)
        {
            lblLoginError.Text = message;
            lblLoginError.Visible = true;
        }

        private bool Authenticate(string username, string password)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_Master_User_Authenticate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 30; // Set timeout

                        // Add parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        con.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            int codereturn = Convert.ToInt32(result);
                            return codereturn == 1;
                        }

                        return false;
                    }
                }
            }
            catch (SqlException)
            {
                // Re-throw to be handled by calling method
                throw;
            }
            catch (Exception)
            {
                // Re-throw to be handled by calling method
                throw;
            }
        }

        // SHA256 hash method for future use if needed
        private string ComputeSha256Hash(string rawData)
        {
            try
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                    StringBuilder builder = new StringBuilder();
                    foreach (byte b in bytes)
                    {
                        builder.Append(b.ToString("x2")); // Convert to hexadecimal
                    }
                    return builder.ToString();
                }
            }
            catch (Exception)
            {
                // Handle hashing errors
                throw new Exception("Error occurred while processing password.");
            }
        }

        // Input sanitization method (though parameterized queries are preferred)
        private string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return input.Trim()
                       .Replace("'", "''")  // Escape single quotes
                       .Replace("--", "")   // Remove SQL comments
                       .Replace("/*", "")   // Remove SQL comments
                       .Replace("*/", "")   // Remove SQL comments
                       .Replace("xp_", "")  // Remove extended stored procedure calls
                       .Replace("sp_", "");  // Remove stored procedure calls
        }
    }
}