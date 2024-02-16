using AuthService;
using BlogService.Common.Constants;
using BlogService.Data.Entity;
using Grpc.Net.Client;

namespace BlogService.SyncDataServices.Grpc
{
    public class AuthenticationDataClient : IAuthenticationDataClient
    {
        private readonly IConfiguration _configuration;

        public AuthenticationDataClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<UserProfile>?> GetAllUsers()
        {
            Console.WriteLine($"--> Calling gRPC Service {_configuration[AppConstants.GrpcAuthentication]}");
            var channel = GrpcChannel.ForAddress(_configuration[AppConstants.GrpcAuthentication]!);
            var client = new GrpcUsers.GrpcUsersClient(channel);
            var request = new GetAllRequest();

            try
            {
                var replay = await client.GetAllUsersAsync(request);
                return replay.Users.Select(u => new UserProfile() { AuthenticationId = u.UserId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not call gRPC Server: {ex.Message}\n{ex.InnerException?.Message}");
                return null;
            }
        }
    }
}