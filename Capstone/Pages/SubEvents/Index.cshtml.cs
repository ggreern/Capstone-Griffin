using Capstone.Pages.Data_Classes;
using Capstone.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Capstone.Pages.Subevents
{
    public class AddSubEventModel : PageModel
    {
        [BindProperty]
        public SubEvent SubEvent { get; set; }
        [BindProperty]
        public Event Event { get; set; }
        public List<SelectListItem> EventList { get; set; }

        public void OnGet()
        {
            EventList = DBClass.GetAllEvents(); // Implement this to fetch all events
        }

        public IActionResult OnPost()
        {
            HttpContext.Session.SetInt32("SelectedEvent", Event.EventID);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("./NewSubEvent");
        }

        // Add a new action method to handle the View Attendance button click
        public IActionResult OnPostViewAttendance()
        {
            HttpContext.Session.SetInt32("SelectedEvent", Event.EventID);
            // Redirect to the EventAttendance page and pass the selected event ID
            return RedirectToPage("/Events/EventAttendance", new { eventId = Event.EventID });
        }
    }
}

