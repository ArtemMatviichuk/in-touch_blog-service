using BlogService.Common.Dtos.MessageBusDtos;
using BlogService.EventProcessing.Interfaces;
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
        if (grpcClient != null)
        {
            var dtos = await grpcClient.GetAllUsers();
            await InsertUsers(serviceScope.ServiceProvider?.GetService<IEventsService>(), dtos);
        }
    }

    private static async Task InsertUsers(IEventsService? service, IEnumerable<IdPublicIdDto>? dtos)
    {
        if (service is null || dtos is null)
            return;

        await service.ClearNotExistingUsers(dtos);

        foreach (var dto in dtos)
        {
            await service.CreateUser(dto);
        }
    }
}