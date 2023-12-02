using Capstone.Pages.Data_Classes;
using Capstone.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Pages.AssignRooms
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<SelectListItem> Events { get; set; }

        [BindProperty]
        public int SelectedEventId { get; set; }

        [BindProperty]
        public int SelectedRoomId { get; set; } // Property for the selected room ID

        [BindProperty]
        public List<Room> SuggestedRooms { get; set; }

        public void OnGet()
        {
            Events = DBClass.GetAllEvents(); // Assuming DBClass.GetAllEvents() returns List<SelectListItem>
            SuggestedRooms = new List<Room>();
        }

        public void OnPost()
        {
            Events = DBClass.GetAllEvents(); // Reload events for the dropdown
            SuggestedRooms = DBClass.GetSuggestedRooms(SelectedEventId); // Assuming DBClass.GetSuggestedRooms(int eventId) returns List<Room>
        }

        public IActionResult OnPostAssignRoom()
        {
            if (SelectedEventId > 0 && SelectedRoomId > 0)
            {
                DBClass.AssignRoomToEvent(SelectedEventId, SelectedRoomId);
                return RedirectToPage("/Events/Index");
            }

            // Handle invalid input
            ModelState.AddModelError("", "Invalid event or room selection.");
            Events = DBClass.GetAllEvents();
            SuggestedRooms = DBClass.GetSuggestedRooms(SelectedEventId);
            return Page();
        }
    }
}
