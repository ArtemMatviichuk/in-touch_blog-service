namespace BlogService.Data.Entity
{
    public class UserProfile
    {
        public int Id { get; set; }
        public int? AuthenticationId { get; set; }
        public string PublicId { get; set; } = string.Empty;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarPath { get; set; }

        public string? Description { get; set; }

        public DateTime? LastModified { get; set; }
    }
}