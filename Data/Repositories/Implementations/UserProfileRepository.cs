using BlogService.Data.Entity;
using BlogService.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Data.Repositories.Implementations
{
    public class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(BlogContext context)
            : base(context)
        {
        }
    }
}