using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages
{
    public class updateCapsulesModel : PageModel
    {
        public CapsulesInfo capsuleInfo = new CapsulesInfo();
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
                    String sql = "SELECT * FROM Capsules WHERE ID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                capsuleInfo.id = reader.GetInt32(0).ToString();
                                capsuleInfo.Product_Name = reader.GetString(1);
                                capsuleInfo.Output = reader.GetString(2);
                                capsuleInfo.CapsuleSizeMM = reader.GetDecimal(3).ToString();
                                capsuleInfo.MachineDimension = reader.GetString(4);
                                capsuleInfo.ShippingWeightKG = reader.GetDecimal(5).ToString();
                                capsuleInfo.image = reader.IsDBNull(6) ? "" : reader.GetString(6); // Handle NULL case
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

        public IActionResult OnPost(string id, string ProductName, string Output, decimal CapsuleSizeMM, string MachineDimension, decimal ShippingWeightKG, IFormFile? ImageURL)
        {
            if (string.IsNullOrEmpty(ProductName))
            {
                errorMessage = "Product name is required.";
                return Page();
            }
            if (string.IsNullOrEmpty(Output))
            {
                errorMessage = "Output is required.";
                return Page();
            }
            if (CapsuleSizeMM <= 0)
            {
                errorMessage = "Capsule Size MM must be greater than 0.";
                return Page();
            }
            if (string.IsNullOrEmpty(MachineDimension))
            {
                errorMessage = "Machine Dimension is required.";
                return Page();
            }
            if (ShippingWeightKG <= 0)
            {
                errorMessage = "Shipping Weight KG must be greater than 0.";
                return Page();
            }

            string imageUrl = ""; // Store old or new image URL

            try
            {
                string connectionString = "Data Source=Uzair;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // ✅ 1️⃣ Get the existing image URL if no new image is uploaded
                    if (ImageURL == null || ImageURL.Length == 0)
                    {
                        string selectQuery = "SELECT ImageURL FROM Capsules WHERE ID = @id";
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
                    string query = @"UPDATE Capsules 
                                    SET ProductName = @ProductName, 
                                        Output = @Output, 
                                        CapsuleSizeMM = @CapsuleSizeMM, 
                                        MachineDimension = @MachineDimension, 
                                        ShippingWeightKG = @ShippingWeightKG, 
                                        ImageURL = @ImageURL 
                                    WHERE ID = @id";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@ProductName", ProductName);
                        cmd.Parameters.AddWithValue("@Output", Output);
                        cmd.Parameters.AddWithValue("@CapsuleSizeMM", CapsuleSizeMM);
                        cmd.Parameters.AddWithValue("@MachineDimension", MachineDimension);
                        cmd.Parameters.AddWithValue("@ShippingWeightKG", ShippingWeightKG);
                        cmd.Parameters.AddWithValue("@ImageURL", imageUrl); // Keeps old image if no new one is uploaded

                        cmd.ExecuteNonQuery();
                    }
                }

                successMessage = "Capsule updated successfully!";
                return RedirectToPage("/capsules");
            }
            catch (Exception ex)
            {
                errorMessage = "An error occurred: " + ex.Message;
                return Page();
            }
        }
    }
}
