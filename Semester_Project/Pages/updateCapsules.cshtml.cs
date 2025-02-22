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
                String connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select * from Capsules where ID=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                capsuleInfo.id = "" + reader.GetInt32(0);
                                capsuleInfo.Product_Name = reader.GetString(1);
                                capsuleInfo.Output = reader.GetString(2);
                                capsuleInfo.CapsuleSizeMM = reader.GetDecimal(3).ToString();
                                capsuleInfo.MachineDimension = reader.GetString(4);
                                capsuleInfo.ShippingWeightKG = reader.GetDecimal(5).ToString();
                                capsuleInfo.image = reader.GetString(6);
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
        public IActionResult OnPost(string id, string ProductName, string Output, decimal CapsuleSizeMM, string MachineDimension, decimal ShippingWeightKG, IFormFile ImageURL)
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
            if (ImageURL == null || ImageURL.Length == 0)
            {
                errorMessage = "Product image is required.";
                return Page();
            }

           
            // Generate a unique file name for the uploaded image
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageURL.FileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            try
            {
                // Save the uploaded image to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageURL.CopyTo(stream);
                }
                string connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Capsules SET ProductName = @ProductName, Output = @Output, CapsuleSizeMM = @CapsuleSizeMM, MachineDimension = @MachineDimension, ShippingWeightKG = @ShippingWeightKG, ImageURL = @ImageURL WHERE ID = @id";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@ProductName", ProductName);
                        cmd.Parameters.AddWithValue("@Output", Output);
                        cmd.Parameters.AddWithValue("@CapsuleSizeMM", CapsuleSizeMM);
                        cmd.Parameters.AddWithValue("@MachineDimension", MachineDimension);
                        cmd.Parameters.AddWithValue("@ShippingWeightKG", ShippingWeightKG);
                        cmd.Parameters.AddWithValue("@ImageURL", "/images/" + fileName);

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