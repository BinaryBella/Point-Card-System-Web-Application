using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Drawing;

namespace Point_Card_System.DAL
{
    public class CustomerController
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;
        public string Get_customer_code()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Master_Customer_GetLastCode", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    con.Close();

                    return result != null ? result.ToString() : "0000"; // Fallback if needed
                }
            }
        }

        public bool IsCustomerExists(string customerName, string mobileNumber)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_CheckCustomerExists", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Customer_Name", customerName.Trim());
                cmd.Parameters.AddWithValue("@Mobile_Number", mobileNumber.Trim());

                con.Open();
                int existingCount = Convert.ToInt32(cmd.ExecuteScalar());
                return existingCount > 0;
            }
        }



        public string Get_customer_branchcode(string user, string pass)
        {
            // Ensure the connection string is correctly set
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Master_Customer_GetBranchCode", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the SqlCommand
                    cmd.Parameters.AddWithValue("@User", user);  // Assuming the parameter name is @User in the stored procedure
                    cmd.Parameters.AddWithValue("@Pass", pass);  // Assuming the parameter name is @Pass in the stored procedure

                    try
                    {
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        con.Close();

                        // Check for null result and return the branch code, or fallback to "0000"
                        return result.ToString();
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (you can replace this with actual logging if needed)
                        Console.WriteLine(ex.Message);

                        // In case of error, return the fallback value
                        return "0000";
                    }
                }
            }
        }

        public string Get_userid(string user, string pass)
        {
            // Ensure the connection string is correctly set
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Get_userid", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the SqlCommand
                    cmd.Parameters.AddWithValue("@User", user);  // Assuming the parameter name is @User in the stored procedure
                    cmd.Parameters.AddWithValue("@Pass", pass);  // Assuming the parameter name is @Pass in the stored procedure

                    try
                    {
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        con.Close();

                        // Check for null result and return the branch code, or fallback to "0000"
                        return result.ToString();
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (you can replace this with actual logging if needed)
                        Console.WriteLine(ex.Message);

                        // In case of error, return the fallback value
                        return "0000";
                    }
                }
            }
        }


        // Insert Customer
        public void InsertCustomer(Dictionary<string, object> parameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_Master_Customer_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters dynamically from dictionary
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue("@" + param.Key, param.Value);
                }

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //add points
        public void InsertPointsDetails(Dictionary<string, object> parameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_Insert_Points", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters dynamically from dictionary
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue("@" + param.Key, param.Value);
                }

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //public string get_cusname(string phone)
        //{
        //    // Ensure the connection string is correctly set
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("SP_Get_cusname", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            // Add parameters to the SqlCommand
        //            cmd.Parameters.AddWithValue("@phone", phone);  // Assuming the parameter name is @User in the stored procedure
        //            //cmd.Parameters.AddWithValue("@Pass", pass);  // Assuming the parameter name is @Pass in the stored procedure

        //            try
        //            {
        //                con.Open();
        //                object result = cmd.ExecuteScalar();
        //                con.Close();

        //                // Check for null result and return the branch code, or fallback to "0000"
        //                return result.ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                // Log the exception (you can replace this with actual logging if needed)
        //                Console.WriteLine(ex.Message);

        //                // In case of error, return the fallback value
        //                return "0000";
        //            }
        //        }
        //    }
        //}

        public InvoiceDetails get_cusname(string phone)
        {
            InvoiceDetails details = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_Get_cusname", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@phone", phone);

                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            details = new InvoiceDetails
                            {
                                CustomerName = reader["Customer_Name"]?.ToString(),
                                CustomerId = reader["Customer_Id"]?.ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the error (for real app, use a logging library)
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return details;
        }

        //return points
        public class InvoiceDetails
        {
            public string CustomerName { get; set; }
            public string MobileNumber { get; set; }
            public double InvoiceAmount { get; set; }
            public double totalpoints { get; set; }
            public string CustomerId { get; set; }
        }

        public InvoiceDetails GetInvoiceDetails(string inv, string branch)
        {
            InvoiceDetails details = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_Get_InvDetails_Return", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@invnum", inv);
                cmd.Parameters.AddWithValue("@branch", branch);

                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            details = new InvoiceDetails
                            {
                                CustomerName = reader["Customer_Name"]?.ToString(),
                                MobileNumber = reader["Mobile_Number"]?.ToString(),
                                InvoiceAmount = reader["invoice_amount"] != DBNull.Value ? Convert.ToDouble(reader["invoice_amount"]) : 0,
                                CustomerId = reader["Customer_Id"]?.ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the error (for real app, use a logging library)
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return details;
        }

        public void Insert_return_PointsDetails(Dictionary<string, object> parameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_Insert_ReturnPoints", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters dynamically from dictionary
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue("@" + param.Key, param.Value);
                }

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //claim points
        public InvoiceDetails get_cusPoints(string phone)
        {
            InvoiceDetails details = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_Get_claiming_Points", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@phone", phone);

                try
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            details = new InvoiceDetails
                            {
                                CustomerName = reader["Customer_Name"]?.ToString(),
                                MobileNumber = reader["Mobile_Number"]?.ToString(),
                                totalpoints = reader["Available_Points"] != DBNull.Value ? Convert.ToDouble(reader["Available_Points"]) : 0,
                                CustomerId = reader["Customer_Id"]?.ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the error (for real app, use a logging library)
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return details;
        }

        public void Insert_Claimed_Points(Dictionary<string, object> parameters)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_Insert_ClaimedPoints", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters dynamically from dictionary
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue("@" + param.Key, param.Value);
                }

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Add these methods to your existing CustomerController class

        // Get user level from User_Master table
        public int Get_user_level(string user, string pass)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Get_UserLevel", con)) // You'll need to create this stored procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User", user);
                    cmd.Parameters.AddWithValue("@Pass", pass);

                    try
                    {
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        con.Close();

                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return 0; // Default to 0 if error
                    }
                }
            }
        }

        // Alternative method using direct SQL query if you prefer not to create stored procedure
        public int Get_user_level_direct(string user, string pass)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT user_level FROM User_Master WHERE user_id = @User AND password = @Pass", con))
                {
                    cmd.Parameters.AddWithValue("@User", user);
                    cmd.Parameters.AddWithValue("@Pass", pass);

                    try
                    {
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        con.Close();

                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return 0;
                    }
                }
            }
        }

        // Get all branches for dropdown
        public DataTable GetAllBranches()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT branch_id, branch_name FROM Branch_Master ORDER BY branch_name", con))
                {
                    try
                    {
                        con.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return new DataTable();
                    }
                }
            }
        }

        // Get branch name by branch ID
        public string GetBranchName(string branchId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT branch_name FROM Branch_Master WHERE branch_id = @BranchId", con))
                {
                    cmd.Parameters.AddWithValue("@BranchId", branchId);

                    try
                    {
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        con.Close();

                        return result != null ? result.ToString() : branchId;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return branchId; // Return branch ID if name not found
                    }
                }
            }
        }

        //dashboard
        public int GetRegisteredCustomerCount()
        {
            int customerCount = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetRegisteredCustomerCount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            customerCount = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (implement logging as needed)
                throw new Exception("Error retrieving customer count: " + ex.Message);
            }

            return customerCount;
        }
    }
}