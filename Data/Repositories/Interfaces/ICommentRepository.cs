using BlogService.Data.Entity;

namespace BlogService.Data.Repositories.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment?> GetFullComment(int id);
        Task<IEnumerable<Comment>> GetPostChain(int postId);
        Task<IEnumerable<Comment>> GetChildrenChain(int commentId);
    }
}
