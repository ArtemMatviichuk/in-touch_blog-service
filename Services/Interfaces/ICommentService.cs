using BlogService.Common.Dtos.Comments;

namespace BlogService.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetComments(int postId);
        Task<CommentDto> GetComment(int id);
        Task<CommentDto> CreateComment(int authId, CreateCommentDto dto);
        Task UpdateComment(int authId, int commentId, string? text);
        Task DeleteComment(int authId, int commentId);
    }
}
