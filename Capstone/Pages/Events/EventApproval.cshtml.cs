using Capstone.Pages.Data_Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Capstone.Pages.Events
{
    public class EventApprovalModel : PageModel
    {

        public Event NewEvent { get; set; }


        public void OnGet()
        {
            NewEvent = new Event(); 
           
        }

        
    }
}