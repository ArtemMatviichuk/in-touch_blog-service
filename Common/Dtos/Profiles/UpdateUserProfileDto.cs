namespace BlogService.Common.Dtos.Profiles
{
    public class UpdateUserProfileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public IFormFile? Avatar { get; set; }
        public bool RemoveAvatar { get; set; }

        public string? Description { get; set; }
    }
}
