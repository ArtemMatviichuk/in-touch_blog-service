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

        public async Task<UserProfile?> GetByAuthId(int authId)
        {
            return await _context.Set<UserProfile>()
                .AsTracking()
                .FirstOrDefaultAsync(e => e.AuthenticationId == authId);
        }
    }
}