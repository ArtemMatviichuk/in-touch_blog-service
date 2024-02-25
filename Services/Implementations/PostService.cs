using AutoMapper;
using BlogService.Common.Dtos.Posts;
using BlogService.Common.Exceptions;
using BlogService.Data.Entity;
using BlogService.Data.Repositories.Interfaces;
using BlogService.Services.Interfaces;

namespace BlogService.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        private readonly ILogger<PostService> _logger;

        public PostService(IUserProfileRepository profileRepository, IPostRepository postRepository,
            ICommentRepository commentRepository, IMapper mapper, ILogger<PostService> logger)
        {
            _profileRepository = profileRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PostDto>> GetMyPosts(int authId)
        {
            var user = await _profileRepository.GetByAuthId(authId);
            if (user == null)
                throw new NotFoundException("User not found");

            var posts = await _postRepository.GetByAuthorId(user.Id);
            var dtos = _mapper.Map<IEnumerable<PostDto>>(posts);

            return dtos;
        }

        public async Task<IEnumerable<PostDto>> GetPosts(int authorId)
        {
            var user = await _profileRepository.Get(authorId);
            if (user == null)
                throw new NotFoundException("User not found");

            var posts = await _postRepository.GetByAuthorId(user.Id);
            var dtos = _mapper.Map<IEnumerable<PostDto>>(posts);

            return dtos;
        }

        public async Task<PostDto> GetPost(int postId)
        {
            var post = await _postRepository.GetFullPost(postId);
            if (post == null)
                throw new NotFoundException("Post not found");

            var dto = _mapper.Map<PostDto>(post);

            return dto;
        }

        public async Task<PostDto> CreatePost(int authId, CreatePostDto dto)
        {
            var user = await _profileRepository.GetByAuthId(authId);
            if (user == null)
                throw new NotFoundException("User not found");

            var post = _mapper.Map<Post>(dto);
            post.AuthorId = user.Id;
            post.CreatedDate = DateTime.Now;

            await _postRepository.Add(post);
            await _postRepository.SaveChanges();

            return await GetPost(post.Id);
        }

        public async Task UpdatePost(int authId, int postId, CreatePostDto dto)
        {
            var user = await _profileRepository.GetByAuthId(authId);
            if (user == null)
                throw new NotFoundException("User not found");

            var post = await _postRepository.GetAsTracking(postId);
            if (post == null)
                throw new NotFoundException("Post not found");

            if (user.Id != post.AuthorId)
                throw new AccessDeniedException("You can edit only your own posts");

            _mapper.Map(dto, post);
            post.LastModified = DateTime.Now;

            await _postRepository.SaveChanges();
        }

        public async Task DeletePost(int authId, int postId)
        {
            var user = await _profileRepository.GetByAuthId(authId);
            if (user == null)
                throw new NotFoundException("User not found");

            var post = await _postRepository.Get(postId);
            if (post == null)
                throw new NotFoundException("Post not found");

            if (user.Id != post.AuthorId)
                throw new AccessDeniedException("You can delete only your own posts");

            var comments = await _commentRepository.GetPostChain(postId);
            if (comments.Any())
            {
                _commentRepository.RemoveRange(comments);
            }

            _postRepository.Remove(post);
            await _postRepository.SaveChanges();
        }
    }
}
