using BlogService.Data.Entity;

namespace BlogService.SyncDataServices.Grpc
{
    public interface IAuthenticationDataClient
    {
        Task<IEnumerable<UserProfile>?> GetAllUsers();
    }
}