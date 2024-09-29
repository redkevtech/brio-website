using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

namespace BrioWebsite.Pages
{
    public class BlogModel : PageModel
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; } = DateTime.Now;
        public string ImageFile { get; set; } = string.Empty;
        public string ImageTitle { get; set; } = string.Empty;
        public string ImageAlt { get; set; } = string.Empty;
        public List<string> Content { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();

        public void OnGet(string slug) // The slug is passed automatically via the route parameter
        {
            if (string.IsNullOrEmpty(slug))
            {
                Title = "Error";
                Content = new List<string> { "No blog post found." };
                return;
            }

            var blogPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Blogs", $"{slug}.json");

            if (System.IO.File.Exists(blogPath))
            {
                var jsonData = System.IO.File.ReadAllText(blogPath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true  // Ignore case when deserializing
                };

                try
                {
                    var blogPost = JsonSerializer.Deserialize<BlogPost>(jsonData, options);

                    if (blogPost != null)
                    {
                        Title = blogPost.Title;
                        Author = blogPost.Author;  // Capture the author
                        PublishDate = blogPost.PublishDate;
                        ImageFile = "/Blogs/Images/" + blogPost.ImageFile;  // Capture the image
                        ImageTitle = blogPost.ImageTitle;  // Capture the image
                        ImageAlt = blogPost.ImageAlt;  // Capture the image
                        Content = blogPost.Content;  // Capture the list of paragraphs
                        Tags = blogPost.Tags;  // Capture the list of paragraphs
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                    Content = new List<string> { "There was an error reading the blog post." };
                }
            }
            else
            {
                Title = "Not Found";
                Content = new List<string> { "The requested blog post could not be found." };
            }
        }

        public class BlogPost
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public DateTime PublishDate { get; set; }
            public string ImageFile { get; set; }
            public string ImageTitle { get; set; }
            public string ImageAlt { get; set; }
            public List<string> Content { get; set; }
            public List<string> Tags { get; set; }
        }
    }

}
