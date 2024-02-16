namespace BlogService.Data.Entity
{
    public class UserProfile
    {
        public int Id { get; set; }
        public int AuthenticationId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? AvatarPath { get; set; }
    }
}