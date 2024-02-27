using AutoMapper;
using BlogService.Common.Constants;
using BlogService.Common.Dtos.Data;
using BlogService.Common.Dtos.General;
using BlogService.Common.Dtos.Profiles;
using BlogService.Common.Exceptions;
using BlogService.Data.Entity;
using BlogService.Data.Repositories.Interfaces;
using BlogService.Services.Interfaces;

namespace BlogService.Services.Implementations
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IFilesService _filesService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserProfileService(IUserProfileRepository userProfileRepository, IFilesService filesService,
            IMapper mapper, IConfiguration configuration)
        {
            _userProfileRepository = userProfileRepository;
            _filesService = filesService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IEnumerable<UserProfileDto>> GetProfiles()
        {
            var profiles = await _userProfileRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<UserProfileDto>>(profiles);

            return dtos;
        }

        public async Task<UserProfileDto> GetProfile(int authId)
        {
            var profile = await _userProfileRepository.GetByAuthId(authId);
            if (profile == null)
                throw new NotFoundException("User not found");

            var dto = _mapper.Map<UserProfileDto>(profile);

            return dto;
        }

        public async Task UpdateProfile(int authId, UpdateUserProfileDto dto)
        {
            var profile = await _userProfileRepository.GetByAuthId(authId);
            if (profile == null)
                throw new NotFoundException("User not found");

            _mapper.Map(dto, profile);

            profile.LastModified = DateTime.Now;

            if (dto.Avatar != null)
            {
                string[] supportedTypes = new string[] { ".png", ".jpg", ".jpeg", ".svg", ".tiff" };
                if (!supportedTypes.Any(t => dto.Avatar.FileName.ToLower().EndsWith(t)))
                {
                    throw new ValidationException($"Unsupported type. Only {string.Join(", ", supportedTypes)} types are available");
                }

                RemoveProfileAvatar(profile);

                profile.AvatarPath = await _filesService.SaveFile(_configuration[AppConstants.FilesPath]!, dto.Avatar);
            }
            else if (dto.RemoveAvatar)
            {
                RemoveProfileAvatar(profile);
                profile.AvatarPath = null;
            }

            await _userProfileRepository.SaveChanges();
        }

        public async Task<FileDto> GetProfileAvatar(string publicId)
        {
            var profile = await _userProfileRepository.GetByPublicId(publicId);
            if (profile == null)
                throw new NotFoundException("User not found");

            if (string.IsNullOrWhiteSpace(profile.AvatarPath))
                throw new ValidationException("User does not have avatar");

            var fileDto = await _filesService.GetFile(_configuration[AppConstants.FilesPath]!, profile.AvatarPath);
            if (fileDto == null || fileDto.Bytes == null)
                throw new NotFoundException("Avatar not found");

            return fileDto;
        }

        private void RemoveProfileAvatar(UserProfile profile)
        {
            if (!string.IsNullOrWhiteSpace(profile.AvatarPath))
            {
                File.Delete(Path.Combine(_configuration[AppConstants.FilesPath]!, profile.AvatarPath));
            }
        }
    }
}