using Client.ApiClients;
using Client.Config;
using Client.Realtime;
using Client.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Client;

internal static class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    private static async Task Main()
    {
        //var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", false)
            //.AddJsonFile($"appsettings.{environment}.json", true)
            .Build();

        var services = new ServiceCollection();

        var remoteConfig = new RemoteConfig
        {
            ApiBaseUrl = configuration["Api:BaseUrl"]!
        };

        /*try
        {
            remoteConfig = await RemoteConfigLoader.Load();
        }
        catch
        {
            // ignored
        }*/

        ConfigureClientServices(services, remoteConfig);

        await using var serviceProvider = services.BuildServiceProvider();

        ApplicationConfiguration.Initialize();

        var mainForm = serviceProvider.GetRequiredService<LoginForm>();
        System.Windows.Forms.Application.Run(mainForm);
    }

    private static void ConfigureClientServices(IServiceCollection services, RemoteConfig remoteConfig)
    {
        services.AddSingleton(remoteConfig);

        services.AddScoped<IDialogService, DialogService>();

        services.AddTransient<LoginForm>();
        services.AddTransient<ClientForm>();

        services.AddSingleton<UserContext>();

        var apiBaseUrl = remoteConfig.ApiBaseUrl;
        MessageBox.Show(apiBaseUrl);

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
    }
}