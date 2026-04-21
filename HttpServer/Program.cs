using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Application.Utils.Mapper;
using Domain.Entities;
using Domain.Enums;
using HttpServer.Hubs;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HttpServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServerServices(builder.Services, builder);

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<MessengerContext>();
            await DatabaseInitializer.InitializeAsync(db);
        }

        app.MapControllers();
        app.MapHub<ChatHub>("/chatHub");
        app.MapHub<FriendHub>("/friendHub");
        app.MapGet("/health", () => Results.Ok("OK"));

        app.UseSwagger();
        app.UseSwaggerUI();

        await app.RunAsync();
    }

    private static void ConfigureServerServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddControllers();
        services.AddSignalR();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        /*var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        var dataSource = new NpgsqlDataSourceBuilder(connectionString)
            .MapEnum<UserStatus>("user_status")
            .MapEnum<ChatParticipationRole>("chat_participation_role")
            .MapEnum<ChatType>("chat_type")
            .MapEnum<ImageContentType>("image_content_type")
            .Build();

        services.AddDbContext<MessengerContext>(options =>
        {
            options.UseNpgsql(dataSource);

            if (builder.Environment.IsDevelopment())
            {
                options.LogTo(Console.WriteLine, LogLevel.Information);
                options.EnableSensitiveDataLogging();
            }
        });*/

        services.AddDbContext<MessengerContext>(options =>
        {
            var connectionString =
                builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MapEnum<UserStatus>("user_status");
                npgsqlOptions.MapEnum<ChatParticipationRole>("chat_participation_role");
                npgsqlOptions.MapEnum<ChatType>("chat_type");
                npgsqlOptions.MapEnum<ImageContentType>("image_content_type");
            });

            if (builder.Environment.IsDevelopment())
            {
                options.LogTo(Console.WriteLine, LogLevel.Information);
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IFriendRepository, FriendRepository>();
        services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();

        services.AddScoped<IAppMapper, AppMapper>();

        services.AddScoped<IMapper<User, UserInfo>, UserMapper>();
        services.AddScoped<IMapper<Message, MessageInfo>, MessageMapper>();
        services.AddScoped<IMapper<Chat, ChatInfo>, ChatMapper>();
        services.AddScoped<IMapper<ChatParticipant, ChatParticipantInfo>, ParticipantMapper>();
        services.AddScoped<IMapper<FriendRequest, FriendRequestInfo>, FriendRequestMapper>();
        services.AddScoped<IMapper<Image, ImageInfo>, ImageMapper>();

        services.AddScoped<AuthService>();
        services.AddScoped<FriendService>();
        services.AddScoped<ChatService>();
        services.AddScoped<UserService>();

        //services.AddScoped<UserContext>();
    }
}