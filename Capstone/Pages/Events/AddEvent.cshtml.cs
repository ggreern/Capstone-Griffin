using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using Capstone.Pages.Data_Classes;
using Capstone.Pages.DB;
using Microsoft.AspNetCore.Http;

namespace Capstone.Pages.Events
{
    public class AddEventModel : PageModel
    {
        [BindProperty]
        public Event NewEvent { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            TempData["EventData"] = NewEvent;
            RedirectToPage("/EventApproval");
            
        }

        
    }
}
