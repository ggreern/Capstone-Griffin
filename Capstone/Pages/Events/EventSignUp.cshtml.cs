using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Capstone.Pages.DB;

namespace Capstone.Pages.Events
{
    public class EventSignUpModel : PageModel
    {
        public EventDisplayModel EventModel { get; set; }



        public IActionResult OnGet()
        {
            // Retrieve events from the database
            EventModel = new EventDisplayModel
            {
                Events = DBClass.GetEventsList() 
            };
      

            return Page();
        }

        public IActionResult OnPostSignUp(int eventId)
        {
            // Get UserID from session state
            int? userId = HttpContext.Session.GetInt32("userID").Value;

            if (userId.HasValue)
            {
                // Add user to the EventRegistration table
                DBClass.AddEventRegistration(userId.Value, eventId);

                ViewData["SignUpMessage"] = "Event sign-up successful!";
            }
            else
            {
                ViewData["SignUpMessage"] = "User not logged in.";
            }

            // Refresh the page to reflect the updated sign-up status
            return RedirectToPage("/Menu");
        }

    }
}

