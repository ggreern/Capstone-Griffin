using Capstone.Pages.Data_Classes;
using Capstone.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Capstone.Pages.AssignRooms
{
    public class IndexModel : PageModel
    {
        public List<SelectListItem> Events { get; set; }
        public int SelectedEventId { get; set; } // Property to hold the selected event ID
        public List<Room> SuggestedRooms { get; set; }
        //[BindProperty]
        //public List<SelectListItem> Events { get; set; }

        [BindProperty]
        public List<SelectListItem> SubEvents { get; set; }

        //[BindProperty]
        //public List<Room> SuggestedRooms { get; set; }

        public void OnGet()
        {
            Events = DBClass.GetAllEvents();
            SubEvents = DBClass.GetAllSubEvents();
        }

        public void OnPost(string selectedEventId, string selectedSubEventId)
        {
            int estimatedAttendance = 0;
            if (!string.IsNullOrEmpty(selectedEventId))
            {
                estimatedAttendance = DBClass.GetEstimatedAttendanceForEvent(int.Parse(selectedEventId));
            }
            else if (!string.IsNullOrEmpty(selectedSubEventId))
            {
                estimatedAttendance = DBClass.GetEstimatedAttendanceForSubEvent(int.Parse(selectedSubEventId));
            }

            SuggestedRooms = DBClass.GetSuggestedRooms(estimatedAttendance);
        }
    }

}
