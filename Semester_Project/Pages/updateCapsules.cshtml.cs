using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using static System.Formats.Asn1.AsnWriter;


namespace Semester_Project.Pages
{
    public class updateCapsulesModel : PageModel
    {
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet() { }

        public IActionResult OnPost(string ProductName, string Output, decimal CapsuleSizeMM, string MachineDimension, decimal ShippingWeightKG, IFormFile ImageURL)
        {
            if (string.IsNullOrEmpty(ProductName) || string.IsNullOrEmpty(Output) || ImageURL == null || ImageURL.Length == 0)
            {
                ErrorMessage = "All fields are required.";
                return Page();
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageURL.FileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageURL.CopyTo(stream);
                }

                string connectionString = "Data Source=Uzair;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Capsules (ProductName, Output, CapsuleSizeMM, MachineDimension, ShippingWeightKG, ImageURL) VALUES (@ProductName, @Output, @CapsuleSizeMM, @MachineDimension, @ShippingWeightKG, @ImageURL)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", ProductName);
                        cmd.Parameters.AddWithValue("@Output", Output);
                        cmd.Parameters.AddWithValue("@CapsuleSizeMM", CapsuleSizeMM);
                        cmd.Parameters.AddWithValue("@MachineDimension", MachineDimension);
                        cmd.Parameters.AddWithValue("@ShippingWeightKG", ShippingWeightKG);
                        cmd.Parameters.AddWithValue("@ImageURL", "/images/" + fileName);

                        cmd.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "Capsule added successfully!";
                return RedirectToPage("/capsules");
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred: " + ex.Message;
                return Page();
            }
        }

        // Update method
        public IActionResult OnPostUpdate(int ID, string ProductName, string Output, decimal CapsuleSizeMM, string MachineDimension, decimal ShippingWeightKG, IFormFile ImageURL)
        {
            if (ID <= 0)
            {
                ErrorMessage = "Invalid Capsule ID.";
                return Page();
            }

            string imagePath = null;
            string connectionString = "Data Source=Uzair;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // If a new image is uploaded, save it
                    if (ImageURL != null && ImageURL.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageURL.FileName);
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            ImageURL.CopyTo(stream);
                        }

                        imagePath = "/images/" + fileName;
                    }

                    // Update query
                    string query = "UPDATE Capsules SET ProductName = @ProductName, Output = @Output, CapsuleSizeMM = @CapsuleSizeMM, MachineDimension = @MachineDimension, ShippingWeightKG = @ShippingWeightKG";

                    if (imagePath != null)
                    {
                        query += ", ImageURL = @ImageURL";
                    }

                    query += " WHERE ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@ProductName", ProductName);
                        cmd.Parameters.AddWithValue("@Output", Output);
                        cmd.Parameters.AddWithValue("@CapsuleSizeMM", CapsuleSizeMM);
                        cmd.Parameters.AddWithValue("@MachineDimension", MachineDimension);
                        cmd.Parameters.AddWithValue("@ShippingWeightKG", ShippingWeightKG);

                        if (imagePath != null)
                        {
                            cmd.Parameters.AddWithValue("@ImageURL", imagePath);
                        }

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            SuccessMessage = "Capsule updated successfully!";
                            return RedirectToPage("/capsules");
                        }
                        else
                        {
                            ErrorMessage = "No capsule found with the provided ID.";
                            return Page();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred: " + ex.Message;
                return Page();
            }
        }
    }
}
