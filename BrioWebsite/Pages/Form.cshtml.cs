using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using MailKit.Net.Smtp;
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

            // Prepare the email message
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Website Contact", "your-email@example.com"));
            emailMessage.To.Add(new MailboxAddress("Brio Wellness", "info@briowellness.com"));
            emailMessage.Subject = "New Contact Form Submission";

            emailMessage.Body = new TextPart("plain")
            {
                Text = $"Name: {Name}\nEmail: {Email}\nPhone: {Phone}\nMessage: {Message}"
            };

            // Sending email
            using (var client = new SmtpClient())
            {
                // For demo purposes, you can use a simple test SMTP service like smtp.gmail.com or any other SMTP provider
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("your-email@gmail.com", "your-password");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }

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

