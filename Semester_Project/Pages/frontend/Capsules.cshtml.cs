
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semester_Project.Pages.frontend
{
    public class CapsulesModel : PageModel
    {
        public List<CapsuleInfo> showCapsule = new List<CapsuleInfo>();

        public void OnGet()
        {
            try
            {
                // Get the 'id' from the query parameters
                string id = Request.Query["id"];
                Console.WriteLine("Received ID: " + id); // Debugging

                if (string.IsNullOrEmpty(id))
                {
                    Console.WriteLine("Error: ID is null or empty.");
                    return;
                }

                // Database connection string
                string connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Capsules WHERE Id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id); // Parameterized Query for security

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CapsuleInfo capsuleInfo = new CapsuleInfo
                                {
                                    Id = reader.GetInt32(0).ToString(),
                                    Product = reader.GetString(1),
                                    output = reader.GetString(2),
                                    capsuleSizeMM = reader.GetDecimal(3).ToString(),
                                    machineDimension = reader.GetString(4),
                                    shippingWeightKG = reader.GetDecimal(5).ToString(),
                                    Image = reader.GetString(6)
                                };

                                showCapsule.Add(capsuleInfo);
                            }
                        }
                    }
                }

                Console.WriteLine("Capsules Found: " + showCapsule.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class CapsuleInfo
    {
        public string Id { get; set; }
        public string Product { get; set; }
        public string output { get; set; }
        public string capsuleSizeMM { get; set; }
        public string machineDimension { get; set; }
        public string shippingWeightKG { get; set; }
        public string Image { get; set; }
    }
}
