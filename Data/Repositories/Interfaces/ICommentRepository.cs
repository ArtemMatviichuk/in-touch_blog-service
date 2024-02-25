using BlogService.Data.Entity;

namespace BlogService.Data.Repositories.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetPostChain(int postId);
    }
}
