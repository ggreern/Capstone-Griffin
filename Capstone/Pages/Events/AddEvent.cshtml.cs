using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using Capstone.Pages.Data_Classes;
using Capstone.Pages.DB;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Capstone.Pages.Events
{
    public class AddEventModel : PageModel
    {
        [BindProperty]
        public Event NewEvent { get; set; }

        public void OnGet()
        {
            NewEvent = new Event(); 
        }


        public void OnPost()
        {
            DBClass.InsertRequestedEvent(NewEvent);


            //string eventJson = JsonConvert.SerializeObject(NewEvent);
            //TempData["EventData"] = eventJson;
            //RedirectToPage("./EventApproval");
        }
    }
}
