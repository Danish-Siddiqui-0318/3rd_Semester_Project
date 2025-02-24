using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages.frontend
{
    public class CareerModel : PageModel
    {
        public List<PortfolioInfo> showPortfolio = new List<PortfolioInfo>();

        public void OnGet()
        {
            try
            {
                var id = HttpContext.Session.GetInt32("id");
                Console.WriteLine("Session ID: " + id);

                if (id == null)
                {
                    Console.WriteLine("User ID is null. Ensure the session is correctly set.");
                    return;
                }

                String connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM user_portfolio WHERE user_id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id); 

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PortfolioInfo portfolioInfo = new PortfolioInfo
                                {
                                    id = reader.GetInt32(0).ToString(),
                                    description = reader.GetString(1),
                                    user_id = reader.GetInt32(2).ToString()
                                };
                                showPortfolio.Add(portfolioInfo);
                            }
                        }
                    }
                }
                Console.WriteLine("Records fetched: " + showPortfolio.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }
        }
    }

    public class PortfolioInfo
    {
        public string id { get; set; }
        public string description { get; set; }
        public string user_id { get; set; }
    }
}
