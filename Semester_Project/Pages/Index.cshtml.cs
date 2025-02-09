using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;


namespace Semester_Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // Check if user is logged in as admin
            string role = HttpContext.Session.GetString("role");
            if (role != "admin")
            {
                // If the user is not an admin, redirect them to the login page
                Response.Redirect("/Login");
            }

        }

        public IActionResult OnPostLogout()
        {
            // Clear the session data
            HttpContext.Session.Clear();
            // Redirect to login page after logging out
            return RedirectToPage("/Login");
        }

    }
}
