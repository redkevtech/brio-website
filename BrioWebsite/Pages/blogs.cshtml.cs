

using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Xml;

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

            // Generate the sitemap
            //GenerateSitemap();
        }

        public void GenerateSitemap()
        {
            var blogDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "blogs");
            var blogFiles = Directory.GetFiles(blogDirectory, "*.json");

            var sitemapPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sitemap.xml");

            using (XmlWriter writer = XmlWriter.Create(sitemapPath, new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset");
                //writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

                // Main blog listing page
                writer.WriteStartElement("url");
                writer.WriteElementString("loc", "https://www.briowellness.com/blog");
                writer.WriteElementString("lastmod", "2024-09-29");
                writer.WriteElementString("changefreq", "Never");
                writer.WriteElementString("priority", "1.0");
                writer.WriteEndElement();

                // Main blog listing page
                writer.WriteStartElement("url");
                writer.WriteElementString("loc", "https://www.briowellness.com/");
                writer.WriteElementString("lastmod", "2024-09-29");
                writer.WriteElementString("changefreq", "Never");
                writer.WriteElementString("priority", "1.0");
                writer.WriteEndElement();

                // Main blog listing page
                writer.WriteStartElement("url");
                writer.WriteElementString("loc", "https://www.briowellness.com/about");
                writer.WriteElementString("lastmod", "2024-09-29");
                writer.WriteElementString("changefreq", "Never");
                writer.WriteElementString("priority", "0.8");
                writer.WriteEndElement();

                // Main blog listing page
                writer.WriteStartElement("url");
                writer.WriteElementString("loc", "https://www.briowellness.com/plans");
                writer.WriteElementString("lastmod", "2024-09-29");
                writer.WriteElementString("changefreq", "Yearly");
                writer.WriteElementString("priority", "0.8");
                writer.WriteEndElement();

                // Main blog listing page
                writer.WriteStartElement("url");
                writer.WriteElementString("loc", "https://www.briowellness.com/blogs");
                writer.WriteElementString("lastmod", "2024-09-29");
                writer.WriteElementString("changefreq", "Weekly");
                writer.WriteElementString("priority", "0.8");
                writer.WriteEndElement();

                // Add each blog post URL
                foreach (var filePath in blogFiles)
                {
                    var jsonData = System.IO.File.ReadAllText(filePath);
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var blogPost = JsonSerializer.Deserialize<BlogPostSummary>(jsonData, options);

                    if (blogPost != null)
                    {
                        var slug = blogPost.Title.Replace(" ", "-").ToLower(); // Generate slug from title

                        writer.WriteStartElement("url");
                        writer.WriteElementString("loc", $"https://www.briowellness.com/blog/{slug}");
                        writer.WriteElementString("lastmod", blogPost.PublishDate.ToString("yyyy-MM-dd"));
                        writer.WriteElementString("changefreq", "monthly");
                        writer.WriteElementString("priority", "0.6");
                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }

}


