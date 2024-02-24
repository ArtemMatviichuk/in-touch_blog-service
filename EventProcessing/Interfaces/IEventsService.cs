using BlogService.Common.Dtos.MessageBusDtos;

namespace BlogService.EventProcessing.Interfaces
{
    public interface IEventsService
    {
        Task CreateUser(IdDto? dto);
        Task ClearUser(IdDto? dto);
        Task ClearNotExistingUsers(IEnumerable<IdDto> dtos);
    }
}
