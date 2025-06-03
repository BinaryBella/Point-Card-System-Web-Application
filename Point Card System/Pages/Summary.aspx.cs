using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Point_Card_System.Pages
{
    public partial class Summary1 : Page
    {
         string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;

        // Properties to maintain state
        public int CurrentPage
        {
            get { return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1; }
            set { ViewState["CurrentPage"] = value; }
        }

        public int PageSize
        {
            get { return ViewState["PageSize"] != null ? (int)ViewState["PageSize"] : 10; }
            set { ViewState["PageSize"] = value; }
        }

        public int TotalRecords
        {
            get { return ViewState["TotalRecords"] != null ? (int)ViewState["TotalRecords"] : 0; }
            set { ViewState["TotalRecords"] = value; }
        }

        public int TotalPages
        {
            get { return ViewState["TotalPages"] != null ? (int)ViewState["TotalPages"] : 0; }
            set { ViewState["TotalPages"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBranches();
                LoadCustomerData();
            }
        }

        //private void LoadBranches()
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("SP_GetAllBranches", conn))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                conn.Open();

        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    ddlBranch.DataSource = reader;
        //                    ddlBranch.DataTextField = "Branch_Name";
        //                    ddlBranch.DataValueField = "Branch_ID";
        //                    ddlBranch.DataBind();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage("Error loading branches: " + ex.Message, "error");
        //    }
        //}

        private void LoadBranches()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetDistinctBranches", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            ddlBranch.DataSource = reader;
                            ddlBranch.DataTextField = "Branch_Name";
                            ddlBranch.DataValueField = "Branch_ID";
                            ddlBranch.DataBind();
                        }
                    }
                }

                // Debug: Check if branches were loaded
                System.Diagnostics.Debug.WriteLine($"Branches loaded: {ddlBranch.Items.Count - 1}"); // -1 for the "All Branches" item
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading branches: " + ex.Message, "error");
                System.Diagnostics.Debug.WriteLine($"LoadBranches Error: {ex.Message}");

                // Fallback: If stored procedure fails, try direct query
                LoadBranchesDirectQuery();
            }
        }

        private void LoadBranchesDirectQuery()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT DISTINCT 
                    Branch_ID,
                    Branch_ID AS Branch_Name
                FROM Customer_Master 
                WHERE Branch_ID IS NOT NULL 
                AND Branch_ID != ''
                ORDER BY Branch_ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            ddlBranch.DataSource = reader;
                            ddlBranch.DataTextField = "Branch_Name";
                            ddlBranch.DataValueField = "Branch_ID";
                            ddlBranch.DataBind();
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Branches loaded via direct query: {ddlBranch.Items.Count - 1}");
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading branches with direct query: " + ex.Message, "error");
                System.Diagnostics.Debug.WriteLine($"LoadBranchesDirectQuery Error: {ex.Message}");
            }
        }

        private void LoadCustomerData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetCustomerSummary", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        cmd.Parameters.AddWithValue("@PhoneNo", string.IsNullOrEmpty(txtPhoneNo.Text) ? (object)DBNull.Value : txtPhoneNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@BranchID", string.IsNullOrEmpty(ddlBranch.SelectedValue) ? (object)DBNull.Value : ddlBranch.SelectedValue);
                        cmd.Parameters.AddWithValue("@PageNumber", CurrentPage);
                        cmd.Parameters.AddWithValue("@PageSize", PageSize);
                        cmd.Parameters.AddWithValue("@SortColumn", "Customer_Name");
                        cmd.Parameters.AddWithValue("@SortDirection", "ASC");

                        conn.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            adapter.Fill(ds);

                            if (ds.Tables.Count > 0)
                            {
                                // Bind customer data
                                gvCustomers.DataSource = ds.Tables[0];
                                gvCustomers.DataBind();

                                // Get pagination info
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    TotalRecords = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalRecords"]);
                                }

                                // Get pagination details from second result set
                                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                {
                                    TotalPages = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalPages"]);
                                }
                                else
                                {
                                    TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
                                }

                                UpdatePaginationControls();
                                UpdateRecordCount();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading customer data: " + ex.Message, "error");
            }
        }

        private void UpdatePaginationControls()
        {
            // Enable/Disable navigation buttons
            btnFirst.Enabled = CurrentPage > 1;
            btnPrevious.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < TotalPages;
            btnLast.Enabled = CurrentPage < TotalPages;

            // Update CSS classes for disabled buttons
            btnFirst.CssClass = CurrentPage > 1 ? "page-link" : "page-link disabled";
            btnPrevious.CssClass = CurrentPage > 1 ? "page-link" : "page-link disabled";
            btnNext.CssClass = CurrentPage < TotalPages ? "page-link" : "page-link disabled";
            btnLast.CssClass = CurrentPage < TotalPages ? "page-link" : "page-link disabled";

            // Update page info
            lblPageInfo.Text = $"Page {CurrentPage} of {TotalPages}";
        }

        private void UpdateRecordCount()
        {
            int startRecord = (CurrentPage - 1) * PageSize + 1;
            int endRecord = Math.Min(CurrentPage * PageSize, TotalRecords);

            lblRecordCount.Text = $"Showing {startRecord} to {endRecord} of {TotalRecords} records";
        }

        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {
            CurrentPage = 1; // Reset to first page when applying filter
            LoadCustomerData();
        }

        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtPhoneNo.Text = "";
            ddlBranch.SelectedIndex = 0;
            CurrentPage = 1;
            LoadCustomerData();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            CurrentPage = 1; // Reset to first page when changing page size
            LoadCustomerData();
        }

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            LoadCustomerData();
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadCustomerData();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadCustomerData();
            }
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            CurrentPage = TotalPages;
            LoadCustomerData();
        }

        protected void gvCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int customerId = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "EditRecord")
                {
                    // Redirect to edit page or show edit modal
                    Response.Redirect($"EditCustomer.aspx?id={customerId}");
                }
                else if (e.CommandName == "ToggleStatus")
                {
                    ToggleCustomerStatus(customerId);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error processing command: " + ex.Message, "error");
            }
        }

        private void ToggleCustomerStatus(int customerId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Customer_Master SET Active = ~Active WHERE Customer_Id = @CustomerId", conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        conn.Open();

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            ShowMessage("Customer status updated successfully.", "success");
                            LoadCustomerData(); // Refresh the grid
                        }
                        else
                        {
                            ShowMessage("Failed to update customer status.", "error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating customer status: " + ex.Message, "error");
            }
        }

        private void ShowMessage(string message, string type)
        {
            string script = $"alert('{message}');";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", script, true);
        }
    }
}
