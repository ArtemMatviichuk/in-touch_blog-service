using AutoMapper;
using BlogService.Common.Dtos.Data;
using BlogService.Common.Dtos.Profiles;
using BlogService.Data.Entity;

namespace BlogService.Common.MappingProfile
{
    public class BlogMappingProfile : Profile
    {
        public BlogMappingProfile()
        {
            CreateMap<UserProfile, UserProfileDto>()
                .ForMember(e => e.HasAvatar, opt => opt.MapFrom(e => !string.IsNullOrWhiteSpace(e.AvatarPath)));

            CreateMap<UpdateUserProfileDto, UserProfile>()
                .ForMember(e => e.AvatarPath, opt => opt.Ignore());
        }
    }
}
