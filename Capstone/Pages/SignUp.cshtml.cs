using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Capstone.Pages.DB;
using Capstone.Pages.Data_Classes;

namespace Capstone.Pages
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public User NewUser { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnPost()
        {
            // Add user to the User table
            DBClass.AddEventUser(NewUser);

            // Hash the password and add username and hashed password to the HashedCredentials table
            
            DBClass.CreateHashedUser(NewUser.Username, Password);

            return RedirectToPage("/Index");
        }
    }
}



