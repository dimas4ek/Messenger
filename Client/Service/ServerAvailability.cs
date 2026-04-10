using System.Net;
using Client.Config;

namespace Client.Service;

public class ServerAvailability(RemoteConfig remoteConfig)
{
    public async Task<bool> WaitUntilAvailable(int maxAttempts = 3, CancellationToken cancellationToken = default)
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(remoteConfig.ApiBaseUrl);
        httpClient.Timeout = TimeSpan.FromSeconds(5);

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                using var response = await httpClient.GetAsync("health", cancellationToken);

                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
            }
            catch
            {
                // ignored
            }

            if (attempt < maxAttempts)
                await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
        }

        return false;
    }
}