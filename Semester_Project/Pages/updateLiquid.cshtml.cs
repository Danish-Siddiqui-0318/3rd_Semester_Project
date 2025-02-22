using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages
{
    public class updateLiquidModel : PageModel
    {
        public LiquidInfo liquidInfo = new LiquidInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                String connectionString = "Data Source=Uzair;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM LiquidFilling WHERE ID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                liquidInfo.id = reader.GetInt32(0).ToString();
                                liquidInfo.ModelName = reader.GetString(1);
                                liquidInfo.AirPressure = reader.GetDecimal(2).ToString();
                                liquidInfo.AirVolume = reader.GetString(3);
                                liquidInfo.FillingSpeed = reader.GetInt32(4).ToString();
                                liquidInfo.FillingRangeML = reader.GetString(5);
                                liquidInfo.image = reader.IsDBNull(6) ? "" : reader.GetString(6); // Handle NULL case
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public IActionResult OnPost(string id, string ModelName, decimal AirPressure, string AirVolume, int FillingSpeed, string FillingRangeML, IFormFile? ImageURL)
        {
            if (string.IsNullOrEmpty(ModelName))
            {
                errorMessage = "Model Name is required.";
                return Page();
            }
            if (AirPressure <= 0)
            {
                errorMessage = "Air Pressure must be greater than 0.";
                return Page();
            }
            if (string.IsNullOrEmpty(AirVolume))
            {
                errorMessage = "Air Volume is required.";
                return Page();
            }
            if (FillingSpeed <= 0)
            {
                errorMessage = "Filling Speed must be greater than 0.";
                return Page();
            }
            if (string.IsNullOrEmpty(FillingRangeML))
            {
                errorMessage = "Filling Range (ML) is required.";
                return Page();
            }

            string imageUrl = ""; // Store old or new image URL

            try
            {
                string connectionString = "Data Source=Uzair;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // ✅ 1️⃣ Retrieve the existing image URL if no new image is uploaded
                    if (ImageURL == null || ImageURL.Length == 0)
                    {
                        string selectQuery = "SELECT ImageURL FROM LiquidFilling WHERE ID = @id";
                        using (SqlCommand selectCmd = new SqlCommand(selectQuery, connection))
                        {
                            selectCmd.Parameters.AddWithValue("@id", id);
                            var result = selectCmd.ExecuteScalar();
                            imageUrl = result != null ? result.ToString() : ""; // Keep existing image URL
                        }
                    }
                    else
                    {
                        // ✅ 2️⃣ Save new image and update URL
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageURL.FileName);
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            ImageURL.CopyTo(stream);
                        }

                        imageUrl = "/images/" + fileName; // New image URL
                    }

                    // ✅ 3️⃣ Update the database record
                    string query = @"UPDATE LiquidFilling 
                                    SET ModelName = @ModelName, 
                                        AirPressure = @AirPressure, 
                                        AirVolume = @AirVolume, 
                                        FillingSpeed = @FillingSpeed, 
                                        FillingRangeML = @FillingRangeML, 
                                        ImageURL = @ImageURL 
                                    WHERE ID = @id";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@ModelName", ModelName);
                        cmd.Parameters.AddWithValue("@AirPressure", AirPressure);
                        cmd.Parameters.AddWithValue("@AirVolume", AirVolume);
                        cmd.Parameters.AddWithValue("@FillingSpeed", FillingSpeed);
                        cmd.Parameters.AddWithValue("@FillingRangeML", FillingRangeML);
                        cmd.Parameters.AddWithValue("@ImageURL", imageUrl); // Keeps old image if no new one is uploaded

                        cmd.ExecuteNonQuery();
                    }
                }

                successMessage = "Liquid updated successfully!";
                return RedirectToPage("/Liquid");
            }
            catch (Exception ex)
            {
                errorMessage = "An error occurred: " + ex.Message;
                return Page();
            }
        }
    }
}
