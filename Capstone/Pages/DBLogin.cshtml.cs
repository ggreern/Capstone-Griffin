using Capstone.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Capstone.Pages
{
    public class DBLoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Assuming DBClass.HashedParameterLogin and DBClass.StoredProcedureLogin are boolean functions
            if (DBClass.HashedParameterLogin(Username, Password) && DBClass.StoredProcedureLogin(Username, Password))
            {
                HttpContext.Session.SetString("username", Username);

                // Redirect to the "Menu" page
                DBClass.CapDBConn.Close();
                return RedirectToPage("/Menu");
            }

            ViewData["LoginMessage"] = "Username and/or Password is Incorrect";
            DBClass.CapDBConn.Close();
            return Page();
        }

        public IActionResult OnPostLogoutHandler()
        {
            HttpContext.Session.Clear();
            return Page();
        }
    }
}
