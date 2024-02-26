using BlogService.Data.Entity;

namespace BlogService.Data.Repositories.Interfaces
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        Task<UserProfile?> GetByPublicId(string publicId);
        Task<UserProfile?> GetByAuthId(int authId);
    }
}