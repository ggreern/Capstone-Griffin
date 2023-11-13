using Capstone.Pages.Data_Classes;
using Capstone.Pages.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Capstone.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string SearchKeyword { get; set; }
        [BindProperty]
        //public List <Event> x { get; set; }
        public List<Event> Events { get; set; }
        [BindProperty]
        public string Description { get; set; }
        


        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToPage("/DBLogin");
            }
            else
            {
                return Page();
            }
        }


        //public void OnPostSearch()
        //{
         //   HttpContext.Session.SetString("SearchKeyword", SearchKeyword);

         //   Events = new List<Event>();

          //  var eventSearch = DBClass.SearchEvent(SearchKeyword);
         //   while (eventSearch.Read())
         //   {
           //     Events.Add(new Event
          //      {
          //          Description = eventSearch.GetString("Description"),
           //         MeetingRoom = eventSearch.GetString("MeetingRoom")

             //   });
           // }
           // DBClass.Lab2Connection.Close();
       // }

        //public void OnPostSearch()
        //{
        //    HttpContext.Session.SetString("SearchKeyword", SearchKeyword);
        //    var eventSearch = DBClass.SearchEvent(SearchKeyword);
        //    x = new List<Event>();
        //    while (eventSearch.Read())
        //    {
        //        x.Add(
        //            new Event(
        //                eventSearch["Description"].ToString(),
        //                eventSearch["MeetingRoom"].ToString()
        //                ));
        //    }
        //    DBClass.Lab2Connection.Close();
        //}

        //public IActionResult OnPost()
        //{

        //    //var eventSearch = DBClass.SearchEvent(SearchKeyword);
        //    //while (eventSearch.Read())
        //    //{
        //    //    x = eventSearch.GetString("Description");
        //    //}
        //    //return Page();


        //}

    }
}
