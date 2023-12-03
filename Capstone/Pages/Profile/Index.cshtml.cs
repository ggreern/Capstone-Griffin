using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Capstone.Pages.DB;
using Capstone.Pages.Data_Classes;

namespace Capstone.Pages.Profile
{
    public class IndexModel : PageModel
    {
        public User CurrentUser { get; set; }

        public void OnGet()
        {
            int userId = GetCurrentUserId();
            if (userId != -1)
            {

                CurrentUser = DBClass.GetUserById(userId);
            }
            else
            {

            }
        }

        private int GetCurrentUserId()
        {
            // Retrieve the user ID from the session
            int? userId = HttpContext.Session.GetInt32("userID");
            if (userId.HasValue)
            {
                return userId.Value;
            }
            else
            {
                // Return -1 or handle this case as needed
                return -1;
            }
        }
    }
}