using System.Text.Json.Serialization;

namespace API_USER.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string Description { get; set; }


        public string AuthorName { get; set; }
        
        public int AuthorId { get; set; }  

    }
}
