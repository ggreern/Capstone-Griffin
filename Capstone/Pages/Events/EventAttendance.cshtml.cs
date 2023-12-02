using Capstone.Pages.Data_Classes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Capstone.Pages.DB;

namespace Capstone.Pages.Attendance
{
    public class EventAttendanceModel : PageModel
    {
        public List<EventRegistration> EventRegistrations { get; set; }
        public List<User> Users { get; set; }

        public void OnGet(int eventId)
        {
            // Retrieve the event registrations for the specified event
            EventRegistrations = DBClass.GetEventRegistrationsByEventId(eventId);

            // Retrieve the users for the event registrations
            Users = new List<User>();
            foreach (var registration in EventRegistrations)
            {
                var user = DBClass.GetUserById(registration.UserID);
                Users.Add(user);
            }
        }
    }
}

