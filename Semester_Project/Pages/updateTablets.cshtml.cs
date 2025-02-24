using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages
{
    public class updateTabletsModel : PageModel
    {
        public TabletInfo tabletInfo = new TabletInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                String connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select * from Tablets where ID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tabletInfo.id = "" + reader.GetInt32(0);
                                tabletInfo.ModelNumber = reader.GetString(1);
                                tabletInfo.Dies = reader.GetString(2);
                                tabletInfo.MaxPressure = reader.GetDecimal(3).ToString();
                                tabletInfo.MaxDiameterMM = reader.GetDecimal(4).ToString();
                                tabletInfo.MaxDepthFillMM = reader.GetDecimal(5).ToString();
                                tabletInfo.ProductionCapacity = reader.GetString(6);
                                tabletInfo.MachineSize = reader.GetString(7);
                                tabletInfo.NetWeightKG = reader.GetDecimal(8).ToString();
                                tabletInfo.imageUrl = reader.GetString(9);
                                

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

        public IActionResult OnPost(
     string id, string ModelNumber, string Dies, decimal MaxPressure, decimal MaxDiameterMM,
     decimal MaxDepthFillMM, string ProductionCapacity, string MachineSize,
     decimal NetWeightKG, IFormFile? ImageURL) 
        {
            if (string.IsNullOrEmpty(ModelNumber) || string.IsNullOrEmpty(Dies) || MaxPressure <= 0 ||
                MaxDiameterMM <= 0 || MaxDepthFillMM <= 0 || string.IsNullOrEmpty(ProductionCapacity) ||
                string.IsNullOrEmpty(MachineSize) || NetWeightKG <= 0)
            {
                errorMessage = "All required fields must be filled correctly.";
                return Page();
            }

            string imageUrl = ""; 

            try
            {
                string connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    if (ImageURL == null || ImageURL.Length == 0)
                    {
                        string selectQuery = "SELECT ImageURL FROM Tablets WHERE ID = @id";
                        using (SqlCommand selectCmd = new SqlCommand(selectQuery, connection))
                        {
                            selectCmd.Parameters.AddWithValue("@id", id);
                            var result = selectCmd.ExecuteScalar();
                            imageUrl = result != null ? result.ToString() : "";
                        }
                    }
                    else
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageURL.FileName);
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            ImageURL.CopyTo(stream);
                        }

                        imageUrl = "/images/" + fileName; 
                    }

                    // 3️⃣ Update the database record
                    string query = "UPDATE Tablets SET " +
                       "ModelNumber = @ModelNumber, " +
                       "Dies = @Dies, " +
                       "MaxPressure = @MaxPressure, " +
                       "MaxDiameterMM = @MaxDiameterMM, " +
                       "MaxDepthFillMM = @MaxDepthFillMM, " +
                       "ProductionCapacity = @ProductionCapacity, " +
                       "MachineSize = @MachineSize, " +
                       "NetWeightKG = @NetWeightKG, " +
                       "ImageURL = @ImageURL " + 
                       "WHERE ID = @id";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@ModelNumber", ModelNumber);
                        cmd.Parameters.AddWithValue("@Dies", Dies);
                        cmd.Parameters.AddWithValue("@MaxPressure", MaxPressure);
                        cmd.Parameters.AddWithValue("@MaxDiameterMM", MaxDiameterMM);
                        cmd.Parameters.AddWithValue("@MaxDepthFillMM", MaxDepthFillMM);
                        cmd.Parameters.AddWithValue("@ProductionCapacity", ProductionCapacity);
                        cmd.Parameters.AddWithValue("@MachineSize", MachineSize);
                        cmd.Parameters.AddWithValue("@NetWeightKG", NetWeightKG);
                        cmd.Parameters.AddWithValue("@ImageURL", imageUrl); 

                        cmd.ExecuteNonQuery();
                    }
                }

                successMessage = "Tablet updated successfully!";
                return RedirectToPage("/Tablets");
            }
            catch (Exception ex)
            {
                errorMessage = "An error occurred: " + ex.Message;
                return Page();
            }
        }

    }
}
