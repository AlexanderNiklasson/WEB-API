namespace API_USER.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }
        
        public string AuthorId { get; set; }  
    }
}
