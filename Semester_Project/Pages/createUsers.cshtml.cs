using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace Semester_Project.Pages
{
    public class createUsersModel : PageModel
    {
        public UserInfo userInfo = new UserInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {

        }

        public void OnPost()
        {
            userInfo.name = Request.Form["name"];
            userInfo.email = Request.Form["email"];
            userInfo.password = Request.Form["password"];
            userInfo.role = Request.Form["role"];
            if (userInfo.name.Length == 0 || userInfo.email.Length == 0 || userInfo.password.Length == 0 || userInfo.role.Length == 0)
            {
                errorMessage = "All Fields should be filled";
                return;
            }

            try
            {
                String connectionString = "Data Source=Uzair;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "insert into Users(name,email,password,role) values(@name,@email,@password,@role)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", userInfo.name);
                        command.Parameters.AddWithValue("@email", userInfo.email);
                        command.Parameters.AddWithValue("@password", userInfo.password);
                        command.Parameters.AddWithValue("@role", userInfo.role);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            successMessage = "New User Added Suuccesfully";
            userInfo.name = "";
            userInfo.email = "";
            userInfo.password = "";
            userInfo.role = "";
            Response.Redirect("/Users");

        }
    }

}

