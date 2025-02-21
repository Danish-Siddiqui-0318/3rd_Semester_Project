using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages
{
    public class PortfolioModel : PageModel
    {
        public List<Portfolio> listPortfolio = new List<Portfolio>();

        public void OnGet()
        {
            string role = HttpContext.Session.GetString("role");
            if (role != "admin")
            {
                Response.Redirect("/Login");
                return; // Exit method to prevent further execution
            }

            try
            {
                string connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT Users.name, Users.email, user_portfolio.id, user_portfolio.description, user_portfolio.created_at FROM Users INNER JOIN user_portfolio ON Users.id = user_portfolio.user_id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows) // Check if data exists
                            {
                                while (reader.Read())
                                {
                                    Portfolio portfolioInfo = new Portfolio();
                                    portfolioInfo.name = reader.GetString(0);
                                    portfolioInfo.email = reader.GetString(1);
                                    portfolioInfo.id = reader.GetInt32(2).ToString();
                                    portfolioInfo.description = reader.GetString(3);
                                    portfolioInfo.created_at = reader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss"); // Format datetime

                                    listPortfolio.Add(portfolioInfo);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No data found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
        }
    }

    public class Portfolio
    {
        public string name;
        public string email;
        public string id;
        public string description;
        public string created_at;
    }
}
