namespace BlogService.Common.Dtos.Posts
{
    public class PostDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }
}
