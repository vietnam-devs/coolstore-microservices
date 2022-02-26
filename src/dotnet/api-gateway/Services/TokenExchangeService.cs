using System.Text.Json.Serialization;
using Gateway.Config;

namespace Gateway.Services;

public class TokenExchangeResponse
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = "";
}

public class TokenExchangeService
{
    private readonly DiscoveryDocument _disco;
    private readonly GatewayConfig _config;

    public TokenExchangeService(GatewayConfig config, DiscoveryDocument disco)
    {
        _disco = disco;
        _config = config;
    }

    public async Task<TokenExchangeResponse?> ExchangeAsync(string? clientId, string? clientSecret, string? scope,
        string? accessToken)
    {
        var payload = new Dictionary<string, string?>
        {
            {"grant_type", "urn:ietf:params:oauth:grant-type:token-exchange"},
            {"scope", scope},
            {"client_id", clientId},
            {"client_secret", clientSecret},
            {"subject_token", accessToken},
            {"subject_token_type", "urn:ietf:params:oauth:token-type:access_token"}
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

        var result = await response.Content.ReadFromJsonAsync<TokenExchangeResponse>();

        return result;
    }
}