using System.Text.Json.Serialization;

namespace Gateway.Services;

public class DiscoveryDocument
{
    [JsonPropertyName("token_endpoint")] public string TokenEndpoint { get; set; } = "";
}

public class DiscoveryService
{
    private const string DiscoUrl = ".well-known/openid-configuration";

    public async Task<DiscoveryDocument> LoadDiscoveryDocument(string authority)
    {
        var httpClient = new HttpClient();

        var url = $"{authority.TrimEnd('/')}/{DiscoUrl.TrimStart('/')}";

        var doc = await httpClient.GetFromJsonAsync<DiscoveryDocument>(url);

        if (doc == null)
        {
            throw new Exception("error loading discovery document from " + url);
        }

        return doc;
    }
}