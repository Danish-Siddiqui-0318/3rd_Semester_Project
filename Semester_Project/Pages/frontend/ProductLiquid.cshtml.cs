using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages.frontend
{
    public class ProductLiquidModel : PageModel
    {
        public List<LiquidInfo> listliquid = new List<LiquidInfo>();

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "select * from LiquidFilling";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                LiquidInfo liquidInfo = new LiquidInfo();
                                liquidInfo.id = "" + reader.GetInt32(0);
                                liquidInfo.ModelName = reader.GetString(1);
                                liquidInfo.AirPressure = reader.GetDecimal(2).ToString();
                                liquidInfo.AirVolume = reader.GetString(3);
                                liquidInfo.FillingSpeed = reader.GetInt32(4).ToString();
                                liquidInfo.FillingRangeML = reader.GetString(5);
                                liquidInfo.image = reader.GetString(6);
                                listliquid.Add(liquidInfo);
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
    public class LiquidInfo
    {
        public String id;
        public String ModelName;
        public String AirPressure;
        public String AirVolume;
        public String FillingSpeed;
        public String FillingRangeML;
        public String image;
    }
}
