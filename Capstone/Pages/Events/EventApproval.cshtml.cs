using Capstone.Pages.Data_Classes;
using Capstone.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Capstone.Pages.Events
{
    public class EventApprovalModel : PageModel
    {
        public List<Event> RequestedEvents { get; set; }

        public Event AddEvent { get; set; }



        public void OnGet()
        {
            RequestedEvents = new List<Event>();
            SqlDataReader getEvents = DBClass.GetRequestedEvents();

            while (getEvents.Read())
            {
                RequestedEvents.Add(new Event
                {
                    Name = getEvents["Name"].ToString(),
                    Address = getEvents["Address"].ToString(),
                    StartDate = getEvents["StartDate"].ToString(),
                    EndDate = getEvents["EndDate"].ToString(),
                    EventType = getEvents["EventType"].ToString(),
                    Description = getEvents["Description"].ToString(),


                    RegistrationCost = (int)Convert.ToDecimal(getEvents["RegistrationCost"]),
                    EstimatedAttendance = (int)Convert.ToDecimal(getEvents["RegistrationCost"])
                    //Not Working


                });


            }

            DBClass.CapDBConn.Close();






        }

        public IActionResult OnPostApprove(int EventID)
        {


            return RedirectToPage("/Events/Index");
        }

        public IActionResult OnPostDeny()
        {
            return RedirectToPage("/Index");
        }


    }
}