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
                String connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "select * from user_portfolio where user_id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PortfolioInfo portfolioInfo = new PortfolioInfo();
                                portfolioInfo.id = "" + reader.GetInt32(0);
                                portfolioInfo.description = reader.GetString(1);
                                portfolioInfo.user_id = ""+reader.GetInt32(2);
                                showPortfolio.Add(portfolioInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.ToString());
            }
        }
    }
    public class PortfolioInfo
    {
        public String id;
        public String description;
        public String user_id;
    }
}
