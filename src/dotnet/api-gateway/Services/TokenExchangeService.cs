using System.Text.Json.Serialization;

namespace Gateway.Services;

public class TokenExchangeResponse
{
    [JsonPropertyName("access_token")] public string AccessToken { get; set; } = "";
}

public class TokenExchangeService
{
    private readonly DiscoveryDocument _disco;
    private readonly IHttpClientFactory _clientFactory;

    public TokenExchangeService(DiscoveryDocument disco, IHttpClientFactory clientFactory)
    {
        _disco = disco;
        _clientFactory = clientFactory;
    }

    public async Task<TokenExchangeResponse?> ExchangeAsync(string? clientId, string? clientSecret, string? scope,
        string? accessToken)
    {
        await _disco.LoadDiscoveryDocument();
        
        var payload = new Dictionary<string, string?>
        {
            {"grant_type", "urn:ietf:params:oauth:grant-type:token-exchange"},
            {"scope", scope},
            {"client_id", clientId},
            {"client_secret", clientSecret},
            {"subject_token", accessToken},
            {"subject_token_type", "urn:ietf:params:oauth:token-type:access_token"}
        };

        // var httpClient = new HttpClient();
        var httpClient = _clientFactory.CreateClient("oidc");

        var request = new HttpRequestMessage
        {
            //RequestUri = new Uri("https://localhost:5001/connect/token"),
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
