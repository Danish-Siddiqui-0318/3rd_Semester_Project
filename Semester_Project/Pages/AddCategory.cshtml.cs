using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using static System.Formats.Asn1.AsnWriter;


namespace Semester_Project.Pages
{
    public class AddCategoryModel : PageModel
    {
        //Store error and success messages
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        // OnGet method for the page load
        public void OnGet() { }

        // OnPost method to handle form submission
        public IActionResult OnPost(string CategoryName, IFormFile CategoryImage)
        {
            if (string.IsNullOrEmpty(CategoryName))
            {
                ErrorMessage = "Category name is required.";
                return Page();
            }

            if (CategoryImage == null || CategoryImage.Length == 0)
            {
                ErrorMessage = "Category image is required.";
                return Page();
            }

            // Generate a unique file name for the uploaded image
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(CategoryImage.FileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            try
            {
                // Save the uploaded image to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    CategoryImage.CopyTo(stream);
                }

                // Save category info to the database
                string connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Categories (CategoryName, CategoryImage) VALUES (@CategoryName, @CategoryImage)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", CategoryName);
                        cmd.Parameters.AddWithValue("@CategoryImage", "/images/" + fileName); // Save the file path (relative to the root)

                        cmd.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "Category added successfully!";
                return RedirectToPage("/Index"); // Redirect to admin dashboard after success
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred: " + ex.Message;
                return Page();
            }
        }

    }
}
