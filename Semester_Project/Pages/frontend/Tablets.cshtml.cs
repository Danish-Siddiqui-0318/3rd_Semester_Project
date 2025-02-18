//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.Data.SqlClient;

//namespace Semester_Project.Pages.frontend
//{
//    public class TabletsModel : PageModel
//    {
//        public List<TabletInfo> listTablets = new List<TabletInfo>();

//        public void OnGet()
//        {
//            try
//            {
//                String id = Request.Query["id"];
//                string connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
//                using (SqlConnection connection = new SqlConnection(connectionString))
//                {
//                    connection.Open();
//                    string sql = "select * from Tablets where id=@id";
//                    using (SqlCommand command = new SqlCommand(sql, connection))
//                    {
//                        using (SqlDataReader reader = command.ExecuteReader())
//                        {
//                            while (reader.Read())
//                            {
//                                TabletInfo tabletInfo = new TabletInfo();
//                                tabletInfo.id = "" + reader.GetInt32(0);
//                                tabletInfo.ModelNumber = reader.GetString(1);
//                                tabletInfo.Dies = reader.GetString(2);
//                                tabletInfo.MaxPressure = reader.GetDecimal(3).ToString();
//                                tabletInfo.MaxDiameterMM = reader.GetDecimal(4).ToString();
//                                tabletInfo.MaxDepthFillMM = reader.GetDecimal(5).ToString();
//                                tabletInfo.ProductionCapacity = reader.GetString(6);
//                                tabletInfo.MachineSize = reader.GetString(7);
//                                tabletInfo.NetWeightKG = reader.GetDecimal(8).ToString();
//                                tabletInfo.imageUrl = reader.GetString(9);
//                                listTablets.Add(tabletInfo);
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Exception : " + ex.ToString());
//            }
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages.frontend
{
    public class TabletsModel : PageModel
    {
        public List<TabletDetails> listTablets = new List<TabletDetails>();

        public void OnGet()
        {
            try
            {
                string id = Request.Query["id"];

                if (string.IsNullOrEmpty(id))
                {
                    Console.WriteLine("Error: Tablet ID is missing.");
                    return; // Stop execution if id is missing
                }

                string connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Tablets WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // ✅ Adding Parameter to Prevent SQL Injection
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TabletDetails tablet = new TabletDetails
                                {
                                    id = reader.GetInt32(0).ToString(),
                                    ModelNumber = reader.GetString(1),
                                    Dies = reader.GetString(2),
                                    MaxPressure = reader.GetDecimal(3).ToString(),
                                    MaxDiameterMM = reader.GetDecimal(4).ToString(),
                                    MaxDepthFillMM = reader.GetDecimal(5).ToString(),
                                    ProductionCapacity = reader.GetString(6),
                                    MachineSize = reader.GetString(7),
                                    NetWeightKG = reader.GetDecimal(8).ToString(),
                                    imageUrl = reader.IsDBNull(9) ? "/images/default.png" : reader.GetString(9) // ✅ Handling Null Image
                                };

                                listTablets.Add(tablet);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }

    public class TabletDetails
    {
        public string id { get; set; }
        public string ModelNumber { get; set; }
        public string Dies { get; set; }
        public string MaxPressure { get; set; }
        public string MaxDiameterMM { get; set; }
        public string MaxDepthFillMM { get; set; }
        public string ProductionCapacity { get; set; }
        public string MachineSize { get; set; }
        public string NetWeightKG { get; set; }
        public string imageUrl { get; set; }
    }
}

