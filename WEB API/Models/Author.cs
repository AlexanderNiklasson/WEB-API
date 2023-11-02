using System.Text.Json.Serialization;

namespace WEB_API.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        
        public ICollection<Post>? Posts { get; set; }
    }
}
