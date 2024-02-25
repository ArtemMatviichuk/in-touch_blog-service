using BlogService.Data;
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

        public async Task<IEnumerable<Comment>> GetPostChain(int postId)
        {
            var comments = await _context.Set<Comment>()
                .Where(e => e.PostId == postId)
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
                .Where(e => e.ParentId.HasValue && parentIds.Contains(e.ParentId.Value))
                .ToListAsync();

            return comments.Concat(await LoadComments(comments.Select(e => e.Id)));
        }
    }
}
