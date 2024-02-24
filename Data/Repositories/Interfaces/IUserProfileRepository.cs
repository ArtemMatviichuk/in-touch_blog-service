using BlogService.Data.Entity;
using BlogService.Data.Repositories.Implementations;

namespace BlogService.Data.Repositories.Interfaces
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        Task<UserProfile?> GetByAuthId(int authId);
    }
}