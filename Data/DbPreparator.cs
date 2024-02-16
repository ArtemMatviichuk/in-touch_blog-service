using BlogService.Data.Entity;
using BlogService.Data.Repositories.Interfaces;
using BlogService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Data;
public static class DbPreparator
{
    public static async Task PrepareDb(IApplicationBuilder app, IConfiguration configuration)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        serviceScope.ServiceProvider.GetService<BlogContext>()?.Database.Migrate();

        var grpcClient = serviceScope.ServiceProvider.GetService<IAuthenticationDataClient>();
        var profiles = await grpcClient?.GetAllUsers();
        await InsertUsers(serviceScope.ServiceProvider?.GetService<IUserProfileRepository>(), profiles);
    }

    private static async Task InsertUsers(IUserProfileRepository? repository, IEnumerable<UserProfile>? profiles)
    {
        if (repository is null || profiles is null)
            return;

        foreach (var profile in profiles)
        {
            if (!await repository.Exists(e => e.AuthenticationId == profile.AuthenticationId))
            {
                await repository.Add(profile);
            }
        }

        await repository.SaveChanges();
    }
}