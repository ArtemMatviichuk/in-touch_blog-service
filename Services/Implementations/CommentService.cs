using BlogService.Common.Dtos.Comments;
using BlogService.Data.Repositories.Interfaces;
using BlogService.Services.Interfaces;

namespace BlogService.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        private readonly ILogger<CommentService> _logger;

        public CommentService(IPostRepository postRepository, ICommentRepository commentRepository, ILogger<CommentService> logger)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _logger = logger;
        }

        public Task<IEnumerable<CommentDto>> GetComments(int postId)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateComment(int authId, CreateCommentDto dto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateComment(int authId, int commentId, string text)
        {
            throw new NotImplementedException();
        }

        public Task DeleteComment(int authId, int commentId)
        {
            throw new NotImplementedException();
        }
    }
}
