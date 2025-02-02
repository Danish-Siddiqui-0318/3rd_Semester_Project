using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages
{
    public class capsulesModel : PageModel
    {
        public List<CapsulesInfo> listCapsules = new List<CapsulesInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
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
                                capsulesInfo.CapsuleSizeMM = reader.GetString(3);
                                capsulesInfo.MachineDimension = reader.GetString(4);
                                capsulesInfo.ShippingWeightKG = reader.GetString(5);
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
