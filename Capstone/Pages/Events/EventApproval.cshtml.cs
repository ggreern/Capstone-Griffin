using Capstone.Pages.Data_Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Capstone.Pages.Events
{
    public class EventApprovalModel : PageModel
    {
        
        public Event NewEvent { get; set; }

        public void OnGet()
        {
            NewEvent = TempData["EventData"] as Event;

        }
    }
}