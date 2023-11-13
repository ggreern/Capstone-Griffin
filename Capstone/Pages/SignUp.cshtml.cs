using Capstone.Pages.Data_Classes;
using Capstone.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Capstone.Pages
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public User NewUser { get; set; }

        public IActionResult OnPost()
        {
            // Add user to the User table
            DBClass.AddEventUser(NewUser);

            // HashedCredentials table is updated separately. You may need to modify this logic based on your schema.
            // Assuming HashedCredentials table has columns like 'Username' and 'Password'

            // Hash the password (you may retrieve it from a different source or generate a random one)
            string password = "your_password"; // Replace with actual password or logic to generate one
            string hashedPassword = PasswordHash.HashPassword(password);

            // Add username and hashed password to the HashedCredentials table
            DBClass.CreateHashedUser(NewUser.Username, hashedPassword);

            return RedirectToPage("/Index"); // Redirect to the desired page after adding the user
        }
    }
}
