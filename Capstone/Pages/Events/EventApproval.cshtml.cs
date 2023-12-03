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
        //public List<Event> RequestedEvents { get; set; }

        public Event AddEvent { get; set; }

        public class EventWithUserInfo : Event
        {
            public string OrganizerName { get; set; }
        }

        public List<EventWithUserInfo> RequestedEvents { get; set; }

        public void OnGet()
        {
            RequestedEvents = new List<EventWithUserInfo>();
            SqlDataReader getEvents = DBClass.GetRequestedEvents();

            while (getEvents.Read())
            {
                int organizerID = Convert.IsDBNull(getEvents["OrganizerID"]) ? 0 : (int)getEvents["OrganizerID"];

                // Skip the current iteration if OrganizerID is null
                if (organizerID == 0)
                {
                    continue;
                }

                // Fetch user information based on OrganizerID
                User organizer = DBClass.GetUserById(organizerID);

                RequestedEvents.Add(new EventWithUserInfo
                {
                    Name = getEvents["Name"].ToString(),
                    Address = getEvents["Address"].ToString(),
                    StartDate = getEvents["StartDate"].ToString(),
                    EndDate = getEvents["EndDate"].ToString(),
                    EventType = getEvents["EventType"].ToString(),
                    Description = getEvents["Description"].ToString(),
                    OrganizerID = organizerID,
                    RegistrationCost = (int)Convert.ToDecimal(getEvents["RegistrationCost"]),
                    EstimatedAttendance = (int)Convert.ToDecimal(getEvents["EstimatedAttendance"]),
                    OrganizerName = $"{organizer.FirstName} {organizer.LastName}", // Include OrganizerName
                });
            }

            DBClass.CapDBConn.Close();
        }


        public IActionResult OnPostApprove(string EventName)
        {
            Event approvedEvent = DBClass.GetRequestedEventDetails(EventName);
            if (approvedEvent != null)
            {
                DBClass.InsertEvent(approvedEvent);

                // Update UserType of the associated organizer to "Organizer"
                int organizerID = approvedEvent.OrganizerID;
                DBClass.UpdateUserType(organizerID, "Organizer");

                return RedirectToPage("/SubEvents/Index");
            }

            return RedirectToPage("/SubEvents/Index");
        }


        public IActionResult OnPostDeny()
        {
            return RedirectToPage("/Index");
        }


    }
}