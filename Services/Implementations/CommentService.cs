using AutoMapper;
using BlogService.Common.Dtos.Comments;
using BlogService.Common.Dtos.Posts;
using BlogService.Common.Exceptions;
using BlogService.Data.Entity;
using BlogService.Data.Repositories.Interfaces;
using BlogService.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace BlogService.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserProfileRepository _profileRepository;
        private readonly IMapper _mapper;

        private readonly ILogger<CommentService> _logger;

        public CommentService(IPostRepository postRepository, ICommentRepository commentRepository,
            IUserProfileRepository profileRepository, IMapper mapper, ILogger<CommentService> logger)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _profileRepository = profileRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CommentDto>> GetComments(int postId)
        {
            var post = await _postRepository.Get(postId);
            if (post == null)
                throw new NotFoundException("Post not found");

            var comments = await _commentRepository.GetPostChain(postId);
            var dtos = _mapper.Map<IEnumerable<CommentDto>>(comments);

            return dtos;
        }

        public async Task<CommentDto> GetComment(int id)
        {
            var comment = await _commentRepository.GetFullComment(id);
            if (comment == null)
                throw new NotFoundException("Comment not found");

            var dto = _mapper.Map<CommentDto>(comment);

            return dto;
        }

        public async Task<CommentDto> CreateComment(int authId, CreateCommentDto dto)
        {
            var user = await _profileRepository.GetByAuthId(authId);
            if (user == null)
                throw new NotFoundException("User not found");

            if (dto.PostId.HasValue)
            {
                dto.ParentId = null;
                var post = await _postRepository.Get(dto.PostId.Value);
                if (post == null)
                    throw new NotFoundException("Post not found");
            }
            else if (dto.ParentId.HasValue)
            {
                var parent = await _commentRepository.Get(dto.ParentId.Value);
                if (parent == null)
                    throw new NotFoundException("Refered comment is not found");
            }
            else
                throw new ValidationException("Parent node is required (post or parent comment)");

            var comment = _mapper.Map<Comment>(dto);
            comment.AuthorId = user.Id;
            comment.CreatedDate = DateTime.Now;

            await _commentRepository.Add(comment);
            await _commentRepository.SaveChanges();

            return await GetComment(comment.Id);
        }

        public async Task UpdateComment(int authId, int commentId, string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ValidationException("Content can not be empty");

            var user = await _profileRepository.GetByAuthId(authId);
            if (user == null)
                throw new NotFoundException("User not found");

            var comment = await _commentRepository.GetAsTracking(commentId);
            if (comment == null)
                throw new NotFoundException("Comment not found");

            if (user.Id != comment.AuthorId)
                throw new AccessDeniedException("You can edit only your own comments");

            comment.Text = text.Trim();
            comment.LastModified = DateTime.Now;

            await _commentRepository.SaveChanges();
        }

        public async Task DeleteComment(int authId, int commentId)
        {
            var user = await _profileRepository.GetByAuthId(authId);
            if (user == null)
                throw new NotFoundException("User not found");

            var comment = await _commentRepository.Get(commentId);
            if (comment == null)
                throw new NotFoundException("Comment not found");

            if (user.Id != comment.AuthorId)
                throw new AccessDeniedException("You can delete only your own comments");

            var comments = await _commentRepository.GetChildrenChain(commentId);
            if (comments.Any())
            {
                _commentRepository.RemoveRange(comments);
            }

            _commentRepository.Remove(comment);
            await _commentRepository.SaveChanges();
        }
    }
}
