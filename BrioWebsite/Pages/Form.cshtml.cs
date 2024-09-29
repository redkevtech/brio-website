using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BrioWebsite.Pages
{
    public class FormModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public bool IsPostBack { get; private set; }

        public void OnGet()
        {
            IsPostBack = false;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid form data");
            }

            IsPostBack = true;

            // Define the path to the CSV file in the root folder
            string csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "ContactFormSubmissions.csv");

            // Prepare the CSV line
            var csvLine = new StringBuilder();
            csvLine.AppendLine($"{Name},{Email},{Phone},{Message}");

            // Check if the file already exists. If not, create it and add headers.
            if (!System.IO.File.Exists(csvFilePath))
            {
                var csvHeader = "Name,Email,Phone,Message";
                await System.IO.File.WriteAllTextAsync(csvFilePath, csvHeader + "\n");
            }

            // Append the new form data to the file
            await System.IO.File.AppendAllTextAsync(csvFilePath, csvLine.ToString());

            // Return a success message in JSON format
            var response = new
            {
                name = Name,
                email = Email,
                phone = Phone,
                message = Message
            };

            return new JsonResult(response);
        }
    }
}
