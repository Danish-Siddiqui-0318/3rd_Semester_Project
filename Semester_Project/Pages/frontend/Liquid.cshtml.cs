using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages.frontend
{
    public class LiquidModel : PageModel
    {
        public List<LiquidDetail> listliquid { get; set; } = new List<LiquidDetail>();

        public void OnGet()
        {
            string? id = Request.Query["id"];

            // Ensure ID is provided before querying
            if (string.IsNullOrEmpty(id))
            {
                Console.WriteLine("Error: ID is missing in query parameters.");
                return;
            }

            string connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM LiquidFilling WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id); // Fix: Secure parameterized query

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listliquid.Add(new LiquidDetail
                                {
                                    Id = reader.GetInt32(0).ToString(),
                                    ModelName = reader.GetString(1),
                                    AirPressure = reader.GetDecimal(2).ToString(),
                                    AirVolume = reader.GetString(3),
                                    FillingSpeed = reader.GetInt32(4).ToString(),
                                    FillingRangeML = reader.GetString(5),
                                    Image = reader.GetString(6)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }

    public class LiquidDetail
    {
        public string Id { get; set; }
        public string ModelName { get; set; }
        public string AirPressure { get; set; }
        public string AirVolume { get; set; }
        public string FillingSpeed { get; set; }
        public string FillingRangeML { get; set; }
        public string Image { get; set; }
    }
}
