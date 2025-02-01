using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System;

namespace Semester_Project.Pages
{
    public class loginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public string ErrorMessage { get; set; }

        public loginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string email, string password)
        {
            // Basic validation of input
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ErrorMessage = "Please fill in both fields.";
                return Page();
            }

            // Connection string from appsettings.json (replace with your actual connection string)
            string connectionString = _configuration.GetConnectionString("Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT id, email, Role FROM Users WHERE email = @email AND password = @password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email",email);
                    cmd.Parameters.AddWithValue("@password", password); // NOTE: Don't store plain passwords in production!

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // User found, store their info in session
                        int userId = Convert.ToInt32(reader["id"]);
                        string userRole = reader["role"].ToString();

                        // Save to session
                        HttpContext.Session.SetInt32("UserId", userId);
                        HttpContext.Session.SetString("email", email);
                        HttpContext.Session.SetString("UserRole", userRole);

                        // Redirect based on role
                        if (userRole == "admin")
                        {
                            return RedirectToPage("/Index");
                        }
                        else
                        {
                            ErrorMessage = "Unauthorized role.";
                            return Page();
                        }
                    }
                    else
                    {
                        ErrorMessage = "Invalid username or password.";
                        return Page();
                    }
                }
            }
        }
    }
}
