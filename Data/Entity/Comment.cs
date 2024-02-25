namespace BlogService.Data.Entity
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }

        public int AuthorId { get; set; }
        public UserProfile? Author { get; set; }
        public int? PostId { get; set; }
        public Post? Post { get; set; }
        public int? ParentId { get; set; }
        public Comment? Parent { get; set; }

        public IEnumerable<Comment>? Comments { get; set; }
    }
}
