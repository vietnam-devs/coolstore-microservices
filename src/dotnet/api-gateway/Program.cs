using System.IdentityModel.Tokens.Jwt;
using Gateway.Config;
using Gateway.Middleware;
using Gateway.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;
using StackExchange.Redis;

IdentityModelEventSource.ShowPII = true; // for dev only
// Disable claim mapping to get claims 1:1 from the tokens
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

// Read config and OIDC discovery document
var config = builder.Configuration.GetGatewayConfig();
builder.Services.AddSingleton<DiscoveryDocument>();

// Configure Services
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("Redis");
    options.InstanceName = "gw-authn";
});
var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetValue<string>("Redis"));
builder.Services.AddDataProtection()
        .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

//builder.Services.AddTransient<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();
builder.AddGateway(config);

builder.Services.AddCors(options =>
{
    options.AddPolicy("api", policy =>
    {
        policy.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin();
    });
}); // for demo only

builder.Services.AddHttpClient("oidc");
    //.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler() // insecure for development
    //{
    //    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    //});

// Build App and add Middleware
var app = builder.Build();

var fordwardedHeaderOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
fordwardedHeaderOptions.KnownNetworks.Clear();
fordwardedHeaderOptions.KnownProxies.Clear();
app.UseForwardedHeaders(fordwardedHeaderOptions);

app.UseHttpsRedirection();

app.UseCors("api"); // for demo only

app.UseGateway();

// Start Gateway
if (string.IsNullOrEmpty(config.Url))
{
    app.Run();
}
else
{
    app.Run(config.Url);
}
