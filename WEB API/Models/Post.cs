using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Text.Json.Serialization;

namespace WEB_API.Models
{
    public class Post
    {
        public int id {  get; set; }
        public string? title { get; set; }
        public string? content { get; set; }
        public string? description { get; set; }
        public int AuthorId { get; set; }

        [JsonIgnore]
        public Author? Author { get; set; }
    }
}
