using BlogService.Common.Dtos.Posts;

namespace BlogService.Services.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetMyPosts(int authId);
        Task<IEnumerable<PostDto>> GetPosts(string publicId);
        Task<PostDto> GetPost(int postId);
        Task<PostDto> CreatePost(int authId, CreatePostDto dto);
        Task UpdatePost(int authId, int postId, CreatePostDto dto);
        Task DeletePost(int authId, int postId);
    }
}
