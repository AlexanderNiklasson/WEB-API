using System.ComponentModel.Design;

namespace WEB_API.Models
{
    public class Post
    {
        public int id {  get; set; }
        public string? title { get; set; }
        public string? content { get; set; }
        public string? description { get; set; }
        
        public int authorID { get; set; }

        public Author Author { get; set; }
    }
}
