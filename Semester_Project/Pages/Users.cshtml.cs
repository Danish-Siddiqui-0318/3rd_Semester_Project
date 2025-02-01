using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace Semester_Project.Pages
{
    public class UsersModel : PageModel
    {
        public List<UserInfo> listUsers = new List<UserInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "select * from Users";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserInfo userInfo = new UserInfo();
                                userInfo.id = "" + reader.GetInt32(0);
                                userInfo.name = reader.GetString(1);
                                userInfo.email = reader.GetString(2);
                                userInfo.password = reader.GetString(3);
                                userInfo.role = reader.GetString(4);
                                userInfo.created_at = reader.GetDateTime(5).ToString();
                                userInfo.updated_at = reader.GetDateTime(6  ).ToString();
                                listUsers.Add(userInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { 
            Console.WriteLine("Exception : "+ ex.ToString());
            }
        }
    }

    public class UserInfo
    {
        public String id;
        public String name;
        public String email;
        public String password;
        public String role;
        public String created_at;
        public String updated_at;

    }
}
