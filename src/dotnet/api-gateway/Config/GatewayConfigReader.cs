namespace Gateway.Config;

public static class GatewayConfigReader
{
    public static GatewayConfig GetGatewayConfig(this ConfigurationManager config)
    {
        var result = new GatewayConfig
        {
            Url = config.GetValue("Gateway:Url", ""),
            SessionTimeoutInMin = config.GetValue("Gateway:SessionTimeoutInMin", 60),
            ApiPath = config.GetValue("Gateway:ApiPath", "/api/"),
            Authority = config.GetValue<string>("OpenIdConnect:Authority"),
            ClientId = config.GetValue<string>("OpenIdConnect:ClientId"),
            ClientSecret = config.GetValue<string>("OpenIdConnect:ClientSecret"),
            Scopes = config.GetValue("OpenIdConnect:Scopes", ""),
            LogoutUrl = config.GetValue("OpenIdConnect:LogoutUrl", ""),
            DownStreamServices = config.GetSection("OpenIdConnect:DownStreamServices").Get<List<DownStreamServiceConfig>>()
        };

        return result;
    }
}
