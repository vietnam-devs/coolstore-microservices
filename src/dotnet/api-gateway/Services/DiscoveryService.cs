using System.Text.Json.Serialization;
using Gateway.Config;

namespace Gateway.Services;

public class DiscoveryDocument
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _config;
    private const string DiscoUrl = ".well-known/openid-configuration";
    
    [JsonPropertyName("token_endpoint")] public string TokenEndpoint { get; set; } = "";

    public DiscoveryDocument(IHttpClientFactory clientFactory, IConfiguration config)
    {
        _clientFactory = clientFactory;
        _config = config;
    }

    public async Task<DiscoveryDocument> LoadDiscoveryDocument()
    {
        var authConfig = _config.GetGatewayConfig();
        var httpClient = _clientFactory.CreateClient("oidc");
        var url = $"{authConfig.Authority.TrimEnd('/')}/{DiscoUrl.TrimStart('/')}";
        var doc = await httpClient.GetFromJsonAsync<DiscoveryDocument>(url);
        if (doc == null)
        {
            throw new Exception("error loading discovery document from " + url);
        }

        return doc;
    }
}

public class DiscoveryService
{
    private const string DiscoUrl = ".well-known/openid-configuration";
    private readonly IHttpClientFactory _clientFactory;

    public DiscoveryService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<DiscoveryDocument> LoadDiscoveryDocument(string authority)
    {
        // ref to this to code https://www.thecodebuzz.com/bypass-ssl-certificate-net-core-windows-linux-ios-net/
        //using (var httpClientHandler = new HttpClientHandler())
        //{
        //    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
        //    using (var client = new HttpClient(httpClientHandler))
        //    {
        //        var url = $"{authority.TrimEnd('/')}/{DiscoUrl.TrimStart('/')}";

        //        var doc = await httpClient.GetFromJsonAsync<DiscoveryDocument>(url);

        //        if (doc == null)
        //        {
        //            throw new Exception("error loading discovery document from " + url);
        //        }

        //        return doc;
        //    }
        //}

        var httpClient = _clientFactory.CreateClient("oidc");
        var url = $"{authority.TrimEnd('/')}/{DiscoUrl.TrimStart('/')}";
        var doc = await httpClient.GetFromJsonAsync<DiscoveryDocument>(url);
        if (doc == null)
        {
            throw new Exception("error loading discovery document from " + url);
        }

        return doc;
    }
}
