using Gateway.Config;
using Gateway.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Gateway.Middleware;

public static class GatewaySetup
{
    public static void AddGateway(this WebApplicationBuilder builder, GatewayConfig config, DiscoveryDocument disco)
    {
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

        builder.Services.AddSingleton(disco);
        builder.Services.AddSingleton(config);
        builder.Services.AddSingleton<TokenRefreshService>();
        builder.Services.AddSingleton<TokenExchangeService>();

        var sessionTimeoutInMin = config.SessionTimeoutInMin;
        builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeoutInMin); });

        builder.Services.AddAntiforgery(setup => { setup.HeaderName = "X-XSRF-TOKEN"; });

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<DiscoveryService>();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("authPolicy", policy => { policy.RequireAuthenticatedUser(); });
        });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(setup =>
        {
            setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionTimeoutInMin);
            setup.SlidingExpiration = true;
        })
        .AddOpenIdConnect(options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.Authority = config.Authority;
            options.ClientId = config.ClientId;
            options.UsePkce = true;
            options.ClientSecret = config.ClientSecret;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.SaveTokens = false;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
            options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
            options.RequireHttpsMetadata = false;
                
            var scopes = config.Scopes;
            var scopeArray = scopes.Split(" ");
            foreach (var scope in scopeArray)
            {
                options.Scope.Add(scope);
            }

            options.Events.OnTokenValidated = (context) =>
            {
                TokenHandler.HandleToken(context);
                return Task.FromResult(0);
            };

            options.Events.OnRedirectToIdentityProviderForSignOut = (context) =>
            {
                LogoutHandler.HandleLogout(context, config);
                return Task.CompletedTask;
            };
        });
    }

    private static void UseYarp(this IEndpointRouteBuilder app)
    {
        app.MapReverseProxy(pipeline => { pipeline.UseGatewayPipeline(); });
    }

    public static void UseGateway(this WebApplication app)
    {
        app.UseRouting();
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCookiePolicy();
        app.UseXsrfCookie();
        app.UseGatewayEndpoints();
        app.UseYarp();
    }
}