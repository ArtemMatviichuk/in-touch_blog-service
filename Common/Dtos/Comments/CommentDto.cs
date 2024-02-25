namespace BlogService.Common.Dtos.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public int? PostId { get; set; }
        public int? ParentId { get; set; }
    }
}
