using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Semester_Project.Pages.frontend
{
    public class AddPortfolioModel : PageModel
    {
        [BindProperty]
        public string Description { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            // This method handles GET requests (optional)
        }

        public IActionResult OnPost()
        {
            try
            {
                var id = HttpContext.Session.GetInt32("id");

                if (id == null)
                {
                    Message = "User is not logged in.";
                    return Page();
                }

                string connectionString = "Data Source=Uzair;Initial Catalog=pharmacy;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO user_portfolio (description, user_id, created_at, updated_at) VALUES (@description, @user_id, GETDATE(), GETDATE())";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@description", Description);
                        command.Parameters.AddWithValue("@user_id", id);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Message = "Portfolio added successfully!";
                        }
                        else
                        {
                            Message = "Failed to add portfolio.";
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
