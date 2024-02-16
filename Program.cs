using BlogService.Services.Interfaces;
using BlogService.Services.Implementations;
using BlogService.Data;
using Microsoft.EntityFrameworkCore;
using BlogService.Common.Constants;
using BlogService.Data.Repositories.Interfaces;
using BlogService.Data.Repositories.Implementations;
using BlogService.EventProcessing;
using BlogService.AsyncDataServices;
using BlogService.SyncDataServices.Grpc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BlogContext>(opt => opt.UseSqlServer(
    builder.Configuration.GetConnectionString(AppConstants.ConnectionStringName)
));

// REPOSITORIES
builder.Services.AddTransient<IUserProfileRepository, UserProfileRepository>();

// SERVICES
builder.Services.AddTransient<IUserProfileService, UserProfileService>();

builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

builder.Services.AddScoped<IAuthenticationDataClient, AuthenticationDataClient>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await DbPreparator.PrepareDb(app, app.Configuration);

app.Run();
