using Capstone.Pages.Data_Classes;
using Capstone.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Capstone.Pages.AdminPage
{
    public class AdminEventReportModel : PageModel
    {
        public List<Event> AdminReportData { get; set; }

        public void OnGet()
        {
            AdminReportData = DBClass.AdminEventList();
        }


        public IActionResult OnPostDelete(int id)
        {
            DBClass.DeleteEvent(id);

            return RedirectToPage("/AdminPage/AdminDashboard");
        }
    }
}

