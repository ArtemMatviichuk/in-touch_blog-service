namespace BlogService.Common.Dtos.Data
{
    public class UserProfileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime? DateOfBirth { get; set; }
        public bool HasAvatar { get; set; }

        public string? Description { get; set; }

        public string PublicId { get; set; } = string.Empty;
    }
}
