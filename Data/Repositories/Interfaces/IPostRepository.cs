using BlogService.Data.Entity;

namespace BlogService.Data.Repositories.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetByAuthorId(int id);
        Task<Post?> GetFullPost(int id);
    }
}
