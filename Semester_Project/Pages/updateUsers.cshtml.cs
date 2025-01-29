using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages
{
    public class updateUsersModel : PageModel
    {
        public UserInfo userInfo=new UserInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            String id=Request.Query["id"];
            try {
                string connectionString = "Data Source=DESKTOP-JCUJJ3K;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString)) { 
                    connection.Open();
                    String sql = "select * from Users where id=@id";
                    using (SqlCommand commmand = new SqlCommand(sql,connection)) {
                        commmand.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = commmand.ExecuteReader()) {
                            if (reader.Read()) {
                            userInfo.id=""+reader.GetInt32(0);
                            userInfo.name=""+reader.GetString(1);
                            userInfo.email=""+reader.GetString(2);
                            userInfo.password=""+reader.GetString(3);
                            userInfo.role=""+reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                errorMessage = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            userInfo.id = Request.Form["id"];
            userInfo.name = Request.Form["name"];
            userInfo.email = Request.Form["email"];
            userInfo.password = Request.Form["password"];
            userInfo.role = Request.Form["role"];
            if (userInfo.id.Length == 0 || userInfo.name.Length == 0 || userInfo.email.Length == 0 || userInfo.password.Length == 0 || userInfo.role.Length == 0)
            {
                errorMessage = "All Fields should be filled";
                return;
            }

            try
            {
                String connectionString = "Data Source=DESKTOP-JCUJJ3K;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Users SET name =@name, email =@email ,password =@password ,role =@role WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", userInfo.id);
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
            Response.Redirect("/Users");
        }

    }
}
