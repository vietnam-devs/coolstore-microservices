using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Middleware;

public static class GatewayEndpoints
{
    public static void UseGatewayEndpoints(this WebApplication app)
    {
        app.UseUserInfoEndpoint();
        app.UseLoginEndpoint();
        app.UseLogoutEndpoint();
        app.UseGatewayStatusEndpoint();
    }

    private static void UseLogoutEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/logout", (string? redirectUrl, HttpContext ctx) =>
        {
            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = "/";
            }

            ctx.Session.Clear();

            var authProps = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };

            var authSchemes = new string[]
            {
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme
            };

            return Results.SignOut(authProps, authSchemes);
        });
    }

    private static void UseLoginEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/login", (string? redirectUrl, HttpContext ctx) =>
        {
            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = "/";
            }

            ctx.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            });
        });
    }

    private static void UseUserInfoEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/userinfo", (ClaimsPrincipal user) =>
        {

            var claims = user.Claims;
            var dict = new Dictionary<string, string>();

            foreach (var entry in claims)
            {
                dict[entry.Type] = entry.Value;
            }

            return dict;
        });
    }

    private static void UseGatewayStatusEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/gw-status", (ClaimsPrincipal user) =>
        {
            var dict = new Dictionary<string, string> {{"version", "1.0.0"}};

            return dict;
        });
    }
}
