using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages.frontend
{
    public class SignupModel : PageModel
    {
        public FrontSignup userInfo = new FrontSignup();
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
            if (userInfo.name.Length == 0 || userInfo.email.Length == 0 || userInfo.password.Length == 0)
            {
                errorMessage = "All Fields should be filled";
                return;
            }

            try
            {
                String connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "insert into Users(name,email,password) values(@name,@email,@password)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", userInfo.name);
                        command.Parameters.AddWithValue("@email", userInfo.email);
                        command.Parameters.AddWithValue("@password", userInfo.password);
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
            Response.Redirect("../frontend/login");
        }
    }
    public class FrontSignup
    {
        public String name;
        public String email;
        public String password;
    }
}
