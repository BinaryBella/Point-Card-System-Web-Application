using System;
using System.Web.UI;
using Point_Card_System.DAL;

namespace Point_Card_System
{
    public partial class _Default : Page
    {
        protected int RegisteredCustomerCount { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardData();
            }
        }

        private void LoadDashboardData()
        {
            try
            {
                CustomerController cuscon = new CustomerController();
                RegisteredCustomerCount = cuscon.GetRegisteredCustomerCount();
            }
            catch (Exception ex)
            {
                // Handle error - you might want to show a user-friendly message
                RegisteredCustomerCount = 0;
                // Log the exception or show error message
                // Response.Write("<script>alert('Error loading dashboard data: " + ex.Message + "');</script>");
            }
        }
    }
}