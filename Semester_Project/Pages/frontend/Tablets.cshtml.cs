using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages.frontend
{
    public class TabletsModel : PageModel
    {
        public List<TabletInfo> listTablets = new List<TabletInfo>();

        public void OnGet()
        {
            try
            {
                String id = Request.Query["id"];
                string connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "select * from Tablets where id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TabletInfo tabletInfo = new TabletInfo();
                                tabletInfo.id = "" + reader.GetInt32(0);
                                tabletInfo.ModelNumber = reader.GetString(1);
                                tabletInfo.Dies = reader.GetString(2);
                                tabletInfo.MaxPressure = reader.GetDecimal(3).ToString();
                                tabletInfo.MaxDiameterMM = reader.GetDecimal(4).ToString();
                                tabletInfo.MaxDepthFillMM = reader.GetDecimal(5).ToString();
                                tabletInfo.ProductionCapacity = reader.GetString(6);
                                tabletInfo.MachineSize = reader.GetString(7);
                                tabletInfo.NetWeightKG = reader.GetDecimal(8).ToString();
                                tabletInfo.imageUrl = reader.GetString(9);
                                listTablets.Add(tabletInfo);
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
}
