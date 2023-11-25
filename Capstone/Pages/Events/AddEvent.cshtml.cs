using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Capstone.Pages.DB;
using Capstone.Pages.Data_Classes;
using System;

namespace Capstone.Pages.Events
{
    public class AddEventModel : PageModel
    {
        [BindProperty]
        public Event NewEvent { get; set; }

        public IActionResult OnGet()
        {
            NewEvent = new Event();
            return Page();
        }

        public IActionResult OnPost()
        {
            // Get the UserID from the session
            int organizerID = HttpContext.Session.GetInt32("userID").Value;

            // Assign the OrganizerID to the OrganizerID property of the new event
            NewEvent.OrganizerID = organizerID;

            // Get UserType directly from the database
            string username = HttpContext.Session.GetString("username");
            string userType = DBClass.GetUserTypeByName(username);

            

            // Redirect based on UserType
            if (userType.Trim().Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Insert requested event
                DBClass.InsertRequestedEvent(NewEvent);

                // Redirect to EventApproval page for Admin
                return RedirectToPage("./EventApproval");
            }
            else
            {
                // Insert requested event
                DBClass.InsertRequestedEvent(NewEvent);

                // Redirect to the current page for non-Admin users
                return RedirectToPage("/SubEvents/Index");
            }
        }


    }
}



