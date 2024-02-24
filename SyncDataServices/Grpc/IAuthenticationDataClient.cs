using BlogService.Common.Dtos.MessageBusDtos;
using BlogService.Data.Entity;

namespace BlogService.SyncDataServices.Grpc
{
    public interface IAuthenticationDataClient
    {
        Task<IEnumerable<IdDto>?> GetAllUsers();
    }
}