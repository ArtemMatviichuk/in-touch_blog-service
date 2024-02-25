using BlogService.Services.Interfaces;
using BlogService.Services.Implementations;
using BlogService.Data;
using Microsoft.EntityFrameworkCore;
using BlogService.Common.Constants;
using BlogService.Data.Repositories.Interfaces;
using BlogService.Data.Repositories.Implementations;
using BlogService.AsyncDataServices;
using BlogService.SyncDataServices.Grpc;
using BlogService.AppSettingsOptions;
using BlogService.EventProcessing.Implementations;
using BlogService.EventProcessing.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using BlogService.Common.Exceptions;
using AutoMapper;
using BlogService.Common.MappingProfile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var securityOptions = new SecurityOptions();
builder.Configuration.Bind(nameof(SecurityOptions), securityOptions);
builder.Services.AddSingleton(securityOptions);

var rabbitMQOptions = new RabbitMQOptions();
builder.Configuration.Bind(nameof(RabbitMQOptions), rabbitMQOptions);
builder.Services.AddSingleton(rabbitMQOptions);

builder.Services.AddDbContext<BlogContext>(opt => opt.UseSqlServer(
    builder.Configuration.GetConnectionString(AppConstants.ConnectionStringName)
));

// REPOSITORIES
builder.Services.AddTransient<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddTransient<IPostRepository, PostRepository>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();

// SERVICES
// api services
builder.Services.AddTransient<IUserProfileService, UserProfileService>();
builder.Services.AddTransient<IPostService, PostService>();

// events services
builder.Services.AddTransient<IEventDeterminator, EventDeterminator>();
builder.Services.AddTransient<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();

// grpc services
builder.Services.AddScoped<IAuthenticationDataClient, AuthenticationDataClient>();

// general purposes
builder.Services.AddTransient<IEventsService, EventsService>();
builder.Services.AddTransient<IFilesService, FilesService>();

builder.Services.AddSingleton(
    new MapperConfiguration(mc =>
        mc.AddProfile(new BlogMappingProfile()))
    .CreateMapper());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "Bearer",

    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme,
                }
            },
            new string[]{}
        }
    });
});

builder.Services
   .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(cfg =>
    {
        var rsa = RSA.Create();
        var key = File.ReadAllText(securityOptions.PublicKeyFilePath);
        rsa.FromXmlString(key);

        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = securityOptions.Issuer,
            ValidAudience = securityOptions.Audience,

            IssuerSigningKey = new RsaSecurityKey(rsa),
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(a => a.Run(async context =>
{
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature?.Error;

    if (exception is CustomException)
    {
        context.Response.StatusCode = (exception as CustomException)!.StatusCode;
    }

    await context.Response.WriteAsJsonAsync(new { error = exception!.Message });
}));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await DbPreparator.PrepareDb(app, app.Configuration);

app.Run();
