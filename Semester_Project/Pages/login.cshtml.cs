using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Text;


namespace Semester_Project.Pages
{
    public class LoginModel : PageModel
    {
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            string email=Request.Form["email"];
            string password=Request.Form["password"];

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) {
                ErrorMessage = "Please fill in all fields";
                return;
            }

            string connectionString = "Data Source=Uzair;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                connection.Open();
                string query = "Select id,name,password,role FROM Users Where email=@email";
                using (SqlCommand cmd = new SqlCommand(query, connection)) { 
                    cmd.Parameters.AddWithValue("email", email);
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                        string storedPassword = reader["password"].ToString();
                        string role = reader["role"].ToString();
                         string name = reader["name"].ToString();
                        int id=(int)reader["id"];

                            if (VerifyPassword(password, storedPassword))
                            {
                                // Save user info to session
                                HttpContext.Session.SetString("name", name);
                                HttpContext.Session.SetInt32("id", id);
                                HttpContext.Session.SetString("role", role);

                                // Redirect based on role
                                if (role == "admin")
                                {
                                    Response.Redirect("/Index");
                                }
                                else {
                                    Response.Redirect("/Login");
                                }
                            }
                            else
                            {
                                ErrorMessage = "Incorrect username or password.";
                            }

                        }
                    }
                }
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return enteredPassword == storedPassword;
        }
    }
}
