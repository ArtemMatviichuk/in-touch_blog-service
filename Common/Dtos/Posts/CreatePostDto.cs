using System.ComponentModel.DataAnnotations;

namespace BlogService.Common.Dtos.Posts
{
    public class CreatePostDto
    {
        [Required]
        [MaxLength(255)]
        public string? Title { get; set; }

        [Required]
        public string? Text { get; set; }
    }
}
