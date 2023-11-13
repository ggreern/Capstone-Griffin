using Capstone.Pages.Data_Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Capstone.Pages.DB;
using Capstone.Pages.Data_Classes;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace Capstone.Pages.Subevents
{
    public class NewSubEventModel : PageModel
    {
        [BindProperty]
        public SubEvent Subevent { get; set; }


        public void OnGet()
        {
          
        }
    }

    public IActionResult OnPost()
    {
        string selectedEvent = HttpContext.Session.GetString("SelectedEvent");
        SqlDataReader c = DBClass.GetEventID(selectedEvent);
        //Get EventID where EventName is equal to SelectedEvent
    }
}
