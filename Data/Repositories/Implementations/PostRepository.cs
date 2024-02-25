using BlogService.Data;
using BlogService.Data.Entity;
using BlogService.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Data.Repositories.Implementations
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(BlogContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Post>> GetByAuthorId(int id)
        {
            return await _context.Set<Post>()
                .Include(e => e.Author)
                .Where(e => e.AuthorId == id)
                .ToListAsync();
        }

        public async Task<Post?> GetFullPost(int id)
        {
            return await _context.Set<Post>()
                .Include(e => e.Author)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
