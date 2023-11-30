using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Capstone.Pages.DB;
using Capstone.Pages.Data_Classes;

namespace Capstone.Pages
{
    public class MenuModel : PageModel
    {
        public string HomePageUrl { get; } = "/Index";
        public string EventsPageUrl { get; } = "/Events";
        public string CalendarPageUrl { get; } = "/Calendar";
        public string MySchedulePageUrl { get; } = "/MySchedule";
        public string ProfilePageUrl { get; } = "/Profile";
        public string SettingsPageUrl { get; } = "/Settings";
        public string SendEmailPageUrl { get; } = "/Events/SendEmail";
        public string EventSignUpUrl { get; } = "/Events/EventSignUp";

        public int OrganizerID { get; private set; }

        public List<Event> UpcomingEvents { get; private set; }

        public IActionResult OnGet()
        {
            // Get the UserID from the session
            int organizerID = HttpContext.Session.GetInt32("userID").Value;
            OrganizerID = organizerID;

            ViewData["OrganizerID"] = organizerID;

            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToPage("/DBLogin");
            }
            else
            {
                // Retrieve upcoming events for the logged-in user
                UpcomingEvents = DBClass.GetEventsForAttendee(organizerID);

                return Page();
            }
        }

        public IActionResult OnPostLogoutHandler()
        {
            HttpContext.Session.Clear();
            return Page();
        }
    }
}

