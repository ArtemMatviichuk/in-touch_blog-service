namespace BlogService.Common.Dtos.Data
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool HasAvatar { get; set; }
    }
}
