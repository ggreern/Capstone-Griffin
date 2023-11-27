using Capstone.Pages.Data_Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;

namespace Capstone.Pages.Events
{
    public class SendEmailModel : PageModel
    {
        [BindProperty]
        public EmailModel EmailForm { get; set; }

        public void OnGet()
        {
            EmailForm = new EmailModel();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                EmailSender.SendEmail(EmailForm);
                ViewData["Message"] = "Email sent successfully.";
            }

            return Page();
        }
    }
}
