namespace Gateway.Config;

public record DownStreamServiceConfig
{
    public string ApiPath { get; set; } = "";
    public string? ClientId { get; init; } = "";
    public string? ClientSecret { get; init; } = "";
    public string? Scope { get; init; } = "";
}

public record GatewayConfig
{
    public string Url { get; init; } = "";
    public int SessionTimeoutInMin { get; init; }
    public string ApiPath { get; init; } = "";
    public string Authority { get; init; } = "";
    public string ExternalAuthority { get; init; } = "";
    public string ClientId { get; init; } = "";
    public string ClientSecret { get; init; } = "";
    public string Scopes { get; init; } = "";
    public string LogoutUrl { get; init; } = "";
    public List<DownStreamServiceConfig> DownStreamServices { get; init; } = new();
}
