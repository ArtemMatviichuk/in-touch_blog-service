using System.Text.Json;
using BlogService.Common.Dtos.MessageBusDtos;
using BlogService.Data.Entity;
using BlogService.Data.Repositories.Interfaces;

namespace BlogService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Dictionary<string, EventType> _eventsDictionary = new()
        {
            { "User_Registered", EventType.UserRegistered },
        };

        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.UserRegistered:
                    await CreateUserProfile(message);
                    break;
                default:
                    break;
            }
        }

        public EventType DetermineEvent(string message)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);

            if (string.IsNullOrWhiteSpace(eventType?.Event))
                return EventType.Undetermined;

            if (!_eventsDictionary.TryGetValue(eventType.Event, out EventType value))
                return EventType.Undetermined;

            return value;
        }

        private async Task CreateUserProfile(string userRegisteredMessage)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IUserProfileRepository>();

            var idDto = JsonSerializer.Deserialize<IdDto>(userRegisteredMessage);
            if (idDto == null)
                return;

            try
            {
                if (!await repository.Exists(e => e.AuthenticationId == idDto.Id))
                {
                    await repository.Add(new UserProfile() { AuthenticationId = idDto.Id });
                    await repository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not create user profile: {ex.Message}");
            }
        }
    }
}