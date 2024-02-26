using BlogService.Data.Entity;
using BlogService.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Data.Repositories.Implementations
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(BlogContext context)
            : base(context)
        {
        }

        public async Task<Comment?> GetFullComment(int id)
        {
            return await _context.Set<Comment>()
                .Include(e => e.Author)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetPostChain(int postId)
        {
            var comments = await _context.Set<Comment>()
                .Include(e => e.Author)
                .Where(e => e.PostId == postId)
                .ToListAsync();

            return comments.Concat(await LoadComments(comments.Select(e => e.Id)));
        }

        public async Task<IEnumerable<Comment>> GetChildrenChain(int commentId)
        {
            var comments = await _context.Set<Comment>()
                .Include(e => e.Author)
                .Where(e => e.ParentId == commentId)
                .ToListAsync();

            return comments.Concat(await LoadComments(comments.Select(e => e.Id)));
        }

        private async Task<IEnumerable<Comment>> LoadComments(IEnumerable<int> parentIds)
        {
            if (parentIds == null || !parentIds.Any())
            {
                return Enumerable.Empty<Comment>();
            }

            var comments = await _context.Set<Comment>()
                .Include(e => e.Author)
                .Where(e => e.ParentId.HasValue && parentIds.Contains(e.ParentId.Value))
                .ToListAsync();

            return comments.Concat(await LoadComments(comments.Select(e => e.Id)));
        }
    }
}
