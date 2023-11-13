using Capstone.Pages.Data_Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Capstone.Pages.DB;
using Capstone.Pages.Data_Classes;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Capstone.Pages.Subevents
{
    public class NewSubEventModel : PageModel
    {
        [BindProperty]
        public SubEvent Subevent { get; set; }


        public void OnGet()
        {
            //string selectedEvent = HttpContext.Session.GetString("SelectedEvent");
            //SqlDataReader c = DBClass.GetEventID(selectedEvent);
        }


        public IActionResult OnPost()
        {
            string hostName = Request.Form["HostID"];

            int hostID = DBClass.GetUserIDByName(hostName);

            Subevent.HostID = hostID;



            int selectedEvent = (int)HttpContext.Session.GetInt32("SelectedEvent");
            Subevent.EventID = selectedEvent;

            // Call your AddSubEvent method
            DBClass.AddSubEvent(Subevent);
            return Page();
        }

    }
}
