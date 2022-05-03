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
    private readonly IHttpClientFactory _clientFactory;
    private readonly GatewayConfig _config;

    public TokenRefreshService(GatewayConfig config, DiscoveryDocument disco, IHttpClientFactory clientFactory)
    {
        _disco = disco;
        _clientFactory = clientFactory;
        _config = config;
    }

    public async Task<RefreshResponse?> RefreshAsync(string refreshToken)
    {
        await _disco.LoadDiscoveryDocument();
        
        var payload = new Dictionary<string, string>
        {
            {"grant_type", "refresh_token"},
            {"refresh_token", refreshToken},
            {"client_id", _config.ClientId},
            {"client_secret", _config.ClientSecret}
        };

        // var httpClient = new HttpClient();
        var httpClient = _clientFactory.CreateClient("oidc");        

        var request = new HttpRequestMessage
        {
            // RequestUri = new Uri("https://localhost:5001/connect/token"),
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
