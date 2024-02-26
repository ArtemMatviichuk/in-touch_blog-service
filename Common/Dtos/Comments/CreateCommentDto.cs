using System.ComponentModel.DataAnnotations;

namespace BlogService.Common.Dtos.Comments
{
    public class CreateCommentDto
    {
        [Required]
        public string? Text { get; set; }
        public int? PostId { get; set; }
        public int? ParentId { get; set; }
    }
}
