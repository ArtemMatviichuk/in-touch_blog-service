using AutoMapper;
using BlogService.Common.Constants;
using BlogService.Common.Dtos.Comments;
using BlogService.Common.Dtos.Data;
using BlogService.Common.Dtos.Posts;
using BlogService.Common.Dtos.Profiles;
using BlogService.Data.Entity;

namespace BlogService.Common.MappingProfile
{
    public class BlogMappingProfile : Profile
    {
        public BlogMappingProfile()
        {
            // PROFILES
            CreateMap<UserProfile, UserProfileDto>()
                .ForMember(e => e.HasAvatar, opt => opt.MapFrom(e => !string.IsNullOrWhiteSpace(e.AvatarPath)));

            CreateMap<UpdateUserProfileDto, UserProfile>()
                .ForMember(e => e.AvatarPath, opt => opt.Ignore());

            // POSTS
            CreateMap<CreatePostDto, Post>()
                .ForMember(e => e.Title, opt => opt.MapFrom(e => e.Title!.Trim()))
                .ForMember(e => e.Text, opt => opt.MapFrom(e => e.Text!.Trim()));
            CreateMap<Post, PostDto>()
                .ForMember(e => e.AuthorName, opt => opt.MapFrom(e => GetAuthorName(e.Author)));

            // COMMENTS
            CreateMap<CreateCommentDto, Comment>()
                .ForMember(e => e.Text, opt => opt.MapFrom(e => e.Text!.Trim()));
            CreateMap<Comment, CommentDto>()
                .ForMember(e => e.AuthorName, opt => opt.MapFrom(e => GetAuthorName(e.Author)));
        }

        private static string GetAuthorName(UserProfile? user)
        {
            if (user == null)
                return LabelConstants.Unknown;

            return user.AuthenticationId.HasValue ? user.FullName : LabelConstants.DeletedAccount;
        }
    }
}
