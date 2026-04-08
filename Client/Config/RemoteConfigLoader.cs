using System.Text.Json;

namespace Client.Config;

public static class RemoteConfigLoader
{
    public static async Task<RemoteConfig> Load()
    {
        using var httpClient = new HttpClient();

        var json = await httpClient.GetStringAsync("https://dimas4ek.github.io/messenger-config/config.json");

        var config = JsonSerializer.Deserialize<RemoteConfig>(json);

        if (config == null || string.IsNullOrWhiteSpace(config.ApiBaseUrl))
            throw new Exception("Не удалось загрузить адрес API");

        return config;
    }
}