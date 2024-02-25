namespace BlogService.Data.Entity
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }

        public int AuthorId { get; set; }
        public UserProfile? Author { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
    }
}
