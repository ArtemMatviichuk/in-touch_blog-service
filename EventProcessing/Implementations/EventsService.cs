using AutoMapper;
using BlogService.Common.Constants;
using BlogService.Common.Dtos.MessageBusDtos;
using BlogService.Data.Entity;
using BlogService.Data.Repositories.Interfaces;
using BlogService.EventProcessing.Interfaces;

namespace BlogService.EventProcessing.Implementations
{
    public class EventsService : IEventsService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EventsService> _logger;

        public EventsService(IUserProfileRepository userProfileRepository,
            IConfiguration configuration, ILogger<EventsService> logger)
        {
            _userProfileRepository = userProfileRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task CreateUser(IdPublicIdDto? dto)
        {
            if (dto == null)
            {
                string message = "Received DTO object is null";
                _logger.LogError(message);

                throw new Exception(message);
            }

            if (!await _userProfileRepository.Exists(e => e.AuthenticationId == dto.Id))
            {
                await _userProfileRepository.Add(new UserProfile() { AuthenticationId = dto.Id, PublicId = dto.PublicId });
                await _userProfileRepository.SaveChanges();
            }
        }

        public async Task ClearUser(IdDto? dto)
        {
            if (dto == null)
            {
                string message = "Received DTO object is null";
                _logger.LogError(message);

                throw new Exception(message);
            }

            var profile = await _userProfileRepository.GetByAuthId(dto.Id);
            if (profile == null)
            {
                _logger.LogInformation($"Profile with authentication ID {dto.Id} not found");
                return;
            }

            profile.AuthenticationId = null;
            profile.FirstName = null;
            profile.LastName = null;
            profile.DateOfBirth = null;
            profile.LastModified = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(profile.AvatarPath))
            {
                File.Delete(Path.Combine(_configuration[AppConstants.FilesPath]!, profile.AvatarPath));
            }

            profile.AvatarPath = null;

            await _userProfileRepository.SaveChanges();
        }

        public async Task ClearNotExistingUsers(IEnumerable<IdDto> dtos)
        {
            var profileIds = dtos.Select(p => p.Id);
            var profilesToRemove = await _userProfileRepository.GetAll(e => e.AuthenticationId.HasValue && !profileIds.Contains(e.AuthenticationId.Value));

            foreach (var profile in profilesToRemove)
            {
                await ClearUser(new IdDto(profile.AuthenticationId!.Value));
            }
        }
    }
}
