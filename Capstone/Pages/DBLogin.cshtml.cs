using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Capstone.Pages.DB;

namespace Capstone.Pages
{
    public class DBLoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnPost()
        {
            if (DBClass.HashedParameterLogin(Username, Password))
            {
                // Get UserID by username
                int userID = DBClass.GetUserIDByName(Username);

                // Set UserID in session state
                HttpContext.Session.SetInt32("userID", userID);

                HttpContext.Session.SetString("username", Username);
                ViewData["LoginMessage"] = "Login Successful!";
                DBClass.CapDBConn.Close();
                return RedirectToPage("Menu");
            }
            else
            {
                ViewData["LoginMessage"] = "Username and/or Password Incorrect";
                DBClass.CapDBConn.Close(); 
                return Page();
            }
        }

        public IActionResult OnPostLogoutHandler()
        {
            HttpContext.Session.Clear();
            return Page();
        }
    }
}

