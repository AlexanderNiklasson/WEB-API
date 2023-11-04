namespace API_USER.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public List<Post> posts { get; set; }
    }
}
