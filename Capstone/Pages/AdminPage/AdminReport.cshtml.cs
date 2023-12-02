using Capstone.Pages.Data_Classes;
using Capstone.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Capstone.Pages.AdminPage
{
    public class AdminReportModel : PageModel
    {
        public List<User> AdminReportData { get; set; }


        public void OnGet()
        {
            AdminReportData = DBClass.GetAllUsers();

        }
    }
}
