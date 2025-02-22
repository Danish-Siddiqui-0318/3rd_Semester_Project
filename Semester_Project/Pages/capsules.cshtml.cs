using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace Semester_Project.Pages
{
    public class CapsulesModel : PageModel
    {
        public List<CapsulesInfo> listCapsules = new List<CapsulesInfo>();
        public void OnGet()
        {
            string role = HttpContext.Session.GetString("role");
            if (role != "admin")
            {
                // If the user is not an admin, redirect them to the login page
                Response.Redirect("/Login");
            }

            try
            {
                String connectionString = "Data Source=Uzair;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "select * from Capsules";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CapsulesInfo capsulesInfo = new CapsulesInfo();
                                capsulesInfo.id = "" + reader.GetInt32(0);
                                capsulesInfo.Product_Name = reader.GetString(1);
                                capsulesInfo.Output = reader.GetString(2);
                                capsulesInfo.CapsuleSizeMM = reader.GetDecimal(3).ToString();
                                capsulesInfo.MachineDimension = reader.GetString(4);
                                capsulesInfo.ShippingWeightKG = reader.GetDecimal(5).ToString();
                                capsulesInfo.image = reader.GetString(6);
                                listCapsules.Add(capsulesInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.ToString());
            }
        }
    }

    public class CapsulesInfo
    {
        public String id;
        public String Product_Name;
        public String Output;
        public String CapsuleSizeMM;
        public String MachineDimension;
        public String ShippingWeightKG;
        public String image;
    }
}
