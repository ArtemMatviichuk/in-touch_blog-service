using BlogService.Common.Dtos.Data;
using BlogService.Common.Dtos.General;
using BlogService.Common.Dtos.Profiles;

namespace BlogService.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<IEnumerable<UserProfileDto>> GetProfiles();
        Task<UserProfileDto> GetProfile(int authId);
        Task UpdateProfile(int authId, UpdateUserProfileDto dto);
        Task<FileDto> GetProfileAvatar(int id);
    }
}