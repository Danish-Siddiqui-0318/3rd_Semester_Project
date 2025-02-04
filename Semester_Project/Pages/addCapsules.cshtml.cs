using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace Semester_Project.Pages
{
    public class addCapsulesModel : PageModel
    {
        // Store error and success messages
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        // OnGet method for the page load
        public void OnGet() { }

        // OnPost method to handle form submission
        public IActionResult OnPost(string ProductName, string Output, decimal CapsuleSizeMM, string MachineDimension, decimal ShippingWeightKG, IFormFile ImageURL)
        {
            if (string.IsNullOrEmpty(ProductName))
            {
                ErrorMessage = "Product name is required.";
                return Page();
            }

            if (string.IsNullOrEmpty(Output))
            {
                ErrorMessage = "Output is required.";
                return Page();
            }

            if (CapsuleSizeMM <= 0)
            {
                ErrorMessage = "Capsule Size MM must be greater than 0.";
                return Page();
            }

            if (string.IsNullOrEmpty(MachineDimension))
            {
                ErrorMessage = "Machine Dimension is required.";
                return Page();
            }

            if (ShippingWeightKG <= 0)
            {
                ErrorMessage = "Shipping Weight KG must be greater than 0.";
                return Page();
            }

            if (ImageURL == null || ImageURL.Length == 0)
            {
                ErrorMessage = "Product image is required.";
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

                // Save product info to the database
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
                        cmd.Parameters.AddWithValue("@ImageURL", "/images/" + fileName); // Save the file path (relative to the root)

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
    }
}

