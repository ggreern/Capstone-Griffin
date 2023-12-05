using Capstone.Pages.Data_Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Capstone.Pages.DB;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Capstone.Pages.SubEvents
{
    public class NewSubEventModel : PageModel
    {
        [BindProperty]
        public SubEvent Subevent { get; set; }

        public List<User> Users { get; set; }

        public void OnGet()
        {
            Users = DBClass.GetUsers();
        }

        public IActionResult OnPost()
        {
            // Retrieve the selected HostID directly from the form
            //int hostID = int.Parse(Request.Form["HostID"]);

            //Subevent.HostID = hostID;

            int selectedEvent = (int)HttpContext.Session.GetInt32("SelectedEvent");
            Subevent.EventID = selectedEvent;

            // Retrieve the selected HostID directly from the form
            int hostID = Subevent.HostID;

            // Update the UserType of the selected host to "Host"
            DBClass.UpdateUserType(hostID, "Host");

            DBClass.AddSubEvent(Subevent);

            // Redirect to the AddSubActivity page in the same folder
            return RedirectToPage("/Events/Index");
        }


    }
}
