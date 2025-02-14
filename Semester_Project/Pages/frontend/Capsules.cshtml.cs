using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;



namespace Semester_Project.Pages.frontend
{
    public class CapsulesModel : PageModel
    {
        public List<CapsuleInfo> showCapsule = new List<CapsuleInfo>();

        public void OnGet()
        {
            try
            {
                String id = Request.Query["id"];
                String connectionString = "Data Source=DANISHPC\\SQLEXPRESS;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "select * from Capsules where Id=@d";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CapsuleInfo capsuleInfo = new CapsuleInfo();
                                capsuleInfo.Id = "" + reader.GetInt32(0);
                                capsuleInfo.Product = reader.GetString(1);
                                capsuleInfo.output = reader.GetString(2);
                                capsuleInfo.capsuleSizeMM = reader.GetDecimal(3).ToString();
                                capsuleInfo.machineDimension = reader.GetString(4);
                                capsuleInfo.shippingWeightKG = reader.GetDecimal(5).ToString();
                                capsuleInfo.Image = reader.GetString(6);
                                showCapsule.Add(capsuleInfo);
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
    public class CapsuleInfo
    {
        public String Id;
        public String Product;
        public String output;
        public String capsuleSizeMM;
        public String machineDimension;
        public String shippingWeightKG;
        public String Image;
    }
}
