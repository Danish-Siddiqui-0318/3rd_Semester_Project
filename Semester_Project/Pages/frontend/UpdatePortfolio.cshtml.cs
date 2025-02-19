using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages.frontend
{
    
    public class UpdatePortfolioModel : PageModel
    {
        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public int PortfolioId { get; set; }

        public string Message { get; set; }

        public void OnGet(int id)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("id");

                if (userId == null)
                {
                    Message = "User is not logged in.";
                    return;
                }

                string connectionString = "Data Source=Uzair;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT id, description FROM user_portfolio WHERE id = @id AND user_id = @user_id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@user_id", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                PortfolioId = reader.GetInt32(0);
                                Description = reader.GetString(1);
                            }
                            else
                            {
                                Message = "Portfolio not found or access denied.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Error: " + ex.Message;
            }
        }

        public IActionResult OnPost()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("id");

                if (userId == null)
                {
                    Message = "User is not logged in.";
                    return Page();
                }

                string connectionString = "Data Source=Uzair;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE user_portfolio SET description = @description, updated_at = GETDATE() WHERE id = @id AND user_id = @user_id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@description", Description);
                        command.Parameters.AddWithValue("@id", PortfolioId);
                        command.Parameters.AddWithValue("@user_id", userId);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Message = "Portfolio updated successfully!";
                        }
                        else
                        {
                            Message = "Update failed. Ensure you own this portfolio.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Error: " + ex.Message;
            }

            return Page();
        }
    }
}
