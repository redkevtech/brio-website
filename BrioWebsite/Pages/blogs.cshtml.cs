

using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace BrioWebsite.Pages
{
    public class Index2Model : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public Index2Model(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
    public class BlogPostSummary
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }
        public string Summary { get; set; }
        public List<string> Tags { get; set; }
        public string Navigation { get; set; }
    }

    public class BlogsModel : PageModel
    {
        public List<BlogPostSummary> BlogPosts { get; set; } = new List<BlogPostSummary>();

        public void OnGet()
        {
            // Path to the blog JSON files in the wwwroot/blogs directory
            var blogDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "blogs");

            // Get all JSON files in the directory
            var blogFiles = Directory.GetFiles(blogDirectory, "*.json");

            // Deserialize each JSON file into a BlogPostSummary object
            foreach (var filePath in blogFiles)
            {
                var jsonData = System.IO.File.ReadAllText(filePath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var blogPost = JsonSerializer.Deserialize<BlogPostSummary>(jsonData, options);

                if (blogPost != null)
                {
                    BlogPosts.Add(blogPost); // Add the blog post to the list
                }
            }
            BlogPosts = BlogPosts.OrderByDescending(b => b.PublishDate).ToList();
        }
    }

}


