using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Capstone.Pages
{
    public class MenuModel : PageModel
    {
        public string HomePageUrl { get; } = "/Index";
        public string EventsPageUrl { get; } = "/Events";
        public string CalendarPageUrl { get; } = "/Calendar";
        public string MySchedulePageUrl { get; } = "/MySchedule";
        public string ProfilePageUrl { get; } = "/Profile";
        public string SettingsPageUrl { get; } = "/Settings";
        public string SendEmailPageUrl { get; } = "/Events/SendEmail";

        public IActionResult OnGet()
        {
            // Get the UserID from the session
            int organizerID = HttpContext.Session.GetInt32("userID").Value;

            ViewData["OrganizerID"] = organizerID;
            


            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToPage("/DBLogin");
            }
            else
            {
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