using System.IdentityModel.Tokens.Jwt;
using Gateway.Config;
using Gateway.Middleware;
using Gateway.Services;
using Microsoft.AspNetCore.Authorization;

// Disable claim mapping to get claims 1:1 from the tokens
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

// Read config and OIDC discovery document
var config = builder.Configuration.GetGatewayConfig();
var discoService = new DiscoveryService();
var disco = await discoService.LoadDiscoveryDocument(config.Authority);

// Configure Services
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("Redis");
    options.InstanceName = "bff-authn";
});
builder.Services.AddTransient<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();
builder.AddGateway(config, disco);

builder.Services.AddCors(options =>
{
    options.AddPolicy("api", policy =>
    {
        policy.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin();
    });
}); // for demo only

// Build App and add Middleware
var app = builder.Build();

app.UseCors("api"); // for demo only

app.UseGateway();

// Start Gateway
if (string.IsNullOrEmpty(config.Url)) {
    app.Run();
}
else {
    app.Run(config.Url);
}