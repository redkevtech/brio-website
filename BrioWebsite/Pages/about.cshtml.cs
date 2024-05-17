using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrioWebsite.Pages
{
    public class Index1Model : PageModel
    {
        public void OnGet()
        {
        }
        //make sliders have navigation buttons        //make sliders have navigation buttons
        public IActionResult OnPost()
        {
            return RedirectToPage("/Index1");
        }
    }
}
