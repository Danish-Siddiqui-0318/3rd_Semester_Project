using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Semester_Project.Pages
{
    public class addLiquidModel : PageModel
    {
        // Store error and success messages
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        // OnGet method for the page load
        public void OnGet() { }

        // OnPost method to handle form submission
        public IActionResult OnPost(string ModelName, decimal AirPressure, string AirVolume, int FillingSpeed, string FillingRangeML, IFormFile ImageURL)
        {
            if (string.IsNullOrEmpty(ModelName))
            {
                ErrorMessage = "Model Name is required.";
                return Page();
            }

            if (AirPressure <= 0)
            {
                ErrorMessage = "Air Pressure must be greater than 0.";
                return Page();
            }

            if (string.IsNullOrEmpty(AirVolume))
            {
                ErrorMessage = "Air Volume is required.";
                return Page();
            }

            if (FillingSpeed <= 0)
            {
                ErrorMessage = "Filling Speed must be greater than 0.";
                return Page();
            }

            if (string.IsNullOrEmpty(FillingRangeML))
            {
                ErrorMessage = "Filling Range (ML) is required.";
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
                string connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO LiquidFilling (ModelName, AirPressure, AirVolume, FillingSpeed, FillingRangeML, ImageURL) 
                                     VALUES (@ModelName, @AirPressure, @AirVolume, @FillingSpeed, @FillingRangeML, @ImageURL)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ModelName", ModelName);
                        cmd.Parameters.AddWithValue("@AirPressure", AirPressure);
                        cmd.Parameters.AddWithValue("@AirVolume", AirVolume);
                        cmd.Parameters.AddWithValue("@FillingSpeed", FillingSpeed);
                        cmd.Parameters.AddWithValue("@FillingRangeML", FillingRangeML);
                        cmd.Parameters.AddWithValue("@ImageURL", "/images/" + fileName); // Save the file path

                        cmd.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "Liquid added successfully!";
                return RedirectToPage("/Liquid");
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred: " + ex.Message;
                return Page();
            }
        }
    }
}
