using Client.Api.Clients;
using Client.Api.Realtime;
using Client.Config;
using Client.Controllers;
using Client.Forms;
using Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Client;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        MainAsync().GetAwaiter().GetResult();
    }

    private static async Task MainAsync()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", false)
            .Build();

        var services = new ServiceCollection();

        var isDevelopment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development"
                            || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        /*var remoteConfig = new RemoteConfig
        {
            ApiBaseUrl = configuration["Api:BaseUrl"]!
        };*/

        var remoteConfig = new RemoteConfig
        {
            ApiBaseUrl = isDevelopment
                ? configuration["Api:DevBaseUrl"]!
                : configuration["Api:BaseUrl"]!
        };

        ConfigureClientServices(services, remoteConfig);

        await using var serviceProvider = services.BuildServiceProvider();

        App.Services = serviceProvider;

        ApplicationConfiguration.Initialize();

        var serverAvailability = serviceProvider.GetRequiredService<ServerAvailability>();
        var isAvailable = await serverAvailability.WaitUntilAvailable();

        if (!isAvailable)
        {
            MessageBox.Show(
                "Сервер недоступен. Попробуйте открыть приложение чуть позже.",
                "Ошибка подключения",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return;
        }

        var mainForm = serviceProvider.GetRequiredService<LoginForm>();
        System.Windows.Forms.Application.Run(mainForm);
    }

    private static void ConfigureClientServices(IServiceCollection services, RemoteConfig remoteConfig)
    {
        services.AddSingleton(remoteConfig);

        services.AddSingleton<ServerAvailability>();

        services.AddScoped<IDialogService, DialogService>();

        services.AddTransient<LoginForm>();
        services.AddTransient<VideoPlayerForm>();
        services.AddTransient<ClientForm>();

        services.AddTransient<LoginController>();
        services.AddTransient<ChatController>();
        services.AddTransient<FriendController>();
        services.AddTransient<ProfileController>();
        //services.AddSingleton<MessageQueueService>();

        services.AddSingleton<UserContext>();

        var apiBaseUrl = remoteConfig.ApiBaseUrl;

        services.AddHttpClient<AuthApiClient>(client => client.BaseAddress = new Uri(apiBaseUrl));
        services.AddHttpClient<FriendApiClient>(client => client.BaseAddress = new Uri(apiBaseUrl));
        services.AddHttpClient<UserApiClient>(client => client.BaseAddress = new Uri(apiBaseUrl));
        services.AddHttpClient<ChatApiClient>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(10);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionIdleTimeout = TimeSpan.FromSeconds(5),
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                ConnectTimeout = TimeSpan.FromSeconds(5)
            });

        services.AddSingleton<ChatRealtimeClient>();
        services.AddSingleton<FriendRealtimeClient>();
    }
}