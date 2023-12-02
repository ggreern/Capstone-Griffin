using Capstone.Pages.Data_Classes;
using Capstone.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Capstone.Pages.AddRooms
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Room Room { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            DBClass.AddRoom(Room);
            return RedirectToPage("/AddRooms/Index"); // Redirect to the Rooms Index page or another appropriate page
        }
    }
}