using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Capstone.Pages.DB;
using Capstone.Pages.Data_Classes;

namespace Capstone.Pages.Subevents
{
    public class AddSubActivityModel : PageModel
    {
        [BindProperty]
        public SubActivity SubActivity { get; set; }

        public List<SubEvent> SubEvents { get; set; }

        public void OnGet()
        {
            // Retrieve the list of SubEvents to populate a dropdown or any other UI element
            SubEvents = DBClass.GetSubEvents();
        }

        public IActionResult OnPost()
        {
            // Retrieve the selected SubEventID directly from the form
            int subEventID = int.Parse(Request.Form["SubEventID"]);

            // Set the SubEventID for the SubActivity
            SubActivity.SubEventID = subEventID;

            // Call your AddSubActivity method
            DBClass.AddSubActivity(SubActivity);

            return Page();
        }
    }
}

