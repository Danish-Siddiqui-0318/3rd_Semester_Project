using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Semester_Project.Pages
{
    public class addTabletsModel : PageModel
    {
        // Store error and success messages
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet() { }

        public IActionResult OnPost(
            string ModelNumber, string Dies, decimal MaxPressure, decimal MaxDiameterMM, 
            decimal MaxDepthFillMM, string ProductionCapacity, string MachineSize, 
            decimal NetWeightKG, IFormFile ImageURL)
        {
            // Validate required fields
            if (string.IsNullOrEmpty(ModelNumber))
            {
                ErrorMessage = "Model Number is required.";
                return Page();
            }

            if (string.IsNullOrEmpty(Dies))
            {
                ErrorMessage = "Dies is required.";
                return Page();
            }

            if (MaxPressure <= 0)
            {
                ErrorMessage = "Max Pressure must be greater than 0.";
                return Page();
            }

            if (MaxDiameterMM <= 0)
            {
                ErrorMessage = "Max Diameter (MM) must be greater than 0.";
                return Page();
            }

            if (MaxDepthFillMM <= 0)
            {
                ErrorMessage = "Max Depth Fill (MM) must be greater than 0.";
                return Page();
            }

            if (string.IsNullOrEmpty(ProductionCapacity))
            {
                ErrorMessage = "Production Capacity is required.";
                return Page();
            }

            if (string.IsNullOrEmpty(MachineSize))
            {
                ErrorMessage = "Machine Size is required.";
                return Page();
            }

            if (NetWeightKG <= 0)
            {
                ErrorMessage = "Net Weight (KG) must be greater than 0.";
                return Page();
            }

            if (ImageURL == null || ImageURL.Length == 0)
            {
                ErrorMessage = "Product image is required.";
                return Page();
            }


            // Generate unique file name for the uploaded image
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageURL.FileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            try
            {
                // Save image to wwwroot/images
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageURL.CopyTo(stream);
                }

                // Database connection
                string connectionString = "Data Source=Uzair;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Tablets (ModelNumber, Dies, MaxPressure, MaxDiameterMM, MaxDepthFillMM, ProductionCapacity, MachineSize, NetWeightKG, ImageURL) " +
                                   "VALUES (@ModelNumber, @Dies, @MaxPressure, @MaxDiameterMM, @MaxDepthFillMM, @ProductionCapacity, @MachineSize, @NetWeightKG, @ImageURL)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ModelNumber", ModelNumber);
                        cmd.Parameters.AddWithValue("@Dies", Dies);
                        cmd.Parameters.AddWithValue("@MaxPressure", MaxPressure);
                        cmd.Parameters.AddWithValue("@MaxDiameterMM", MaxDiameterMM);
                        cmd.Parameters.AddWithValue("@MaxDepthFillMM", MaxDepthFillMM);
                        cmd.Parameters.AddWithValue("@ProductionCapacity", ProductionCapacity);
                        cmd.Parameters.AddWithValue("@MachineSize", MachineSize);
                        cmd.Parameters.AddWithValue("@NetWeightKG", NetWeightKG);
                        cmd.Parameters.AddWithValue("@ImageURL", "/images/" + fileName); // Save relative image path

                        cmd.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "Tablet added successfully!";
                return RedirectToPage("/Tablets");
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred: " + ex.Message;
                return Page();
            }
        }
    }
}
