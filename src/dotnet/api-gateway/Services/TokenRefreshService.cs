using System.Text.Json.Serialization;
using Gateway.Config;

namespace Gateway.Services;

public class RefreshResponse
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = "";

    [JsonPropertyName("id_token")] public string IdToken { get; set; } = "";

    [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; } = "";
    
    [JsonPropertyName("expires")] public long Expires { get; set; }
}

public class TokenRefreshService
{
    private readonly DiscoveryDocument _disco;
    private readonly GatewayConfig _config;

    public TokenRefreshService(GatewayConfig config, DiscoveryDocument disco)
    {
        _disco = disco;
        _config = config;
    }

    public async Task<RefreshResponse?> RefreshAsync(string refreshToken)
    {
        var payload = new Dictionary<string, string>
        {
            {"grant_type", "refresh_token"},
            {"refresh_token", refreshToken},
            {"client_id", _config.ClientId},
            {"client_secret", _config.ClientSecret}
        };

        var httpClient = new HttpClient();

        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(_disco.TokenEndpoint),
            Method = HttpMethod.Post,
            Content = new FormUrlEncodedContent(payload)
        };

        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<RefreshResponse>();

        return result;
    }
}