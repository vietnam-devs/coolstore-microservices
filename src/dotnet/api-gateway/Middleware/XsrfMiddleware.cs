using Gateway.Config;
using Microsoft.AspNetCore.Antiforgery;

namespace Gateway.Middleware;

public static class XsrfMiddleware
{
    public static void UseXsrfCookie(this WebApplication app)
    {
        app.UseXsrfCookieCreator();
        app.UseXsrfCookieChecks();
    }

    private static void UseXsrfCookieCreator(this WebApplication app)
    {
        app.Use(async (ctx, next) =>
        {
            var antiforgery = app.Services.GetService<IAntiforgery>();

            if (antiforgery == null)
            {
                throw new Exception("IAntiforgery service expected!");
            }

            var tokens = antiforgery!.GetAndStoreTokens(ctx);

            if (tokens.RequestToken == null)
            {
                throw new Exception("token expected!");
            }

            ctx.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                new CookieOptions() {
                    HttpOnly = false,
                });

            await next(ctx);
        });
    }

    private static void UseXsrfCookieChecks(this WebApplication app)
    {
        var config = app.Services.GetRequiredService<GatewayConfig>();
        var apiPath = config.ApiPath;

        app.Use(async (ctx, next) =>
        {
            var antiforgery = app.Services.GetService<IAntiforgery>();

            if (antiforgery == null)
            {
                throw new Exception("IAntiforgery service expected!");
            }

            var currentUrl = ctx.Request.Path.ToString().ToLower();
            if (currentUrl.StartsWith(apiPath)
                && !await antiforgery.IsRequestValidAsync(ctx))
            {
                ctx.Response.StatusCode = 400;
                await ctx.Response.WriteAsJsonAsync(new
                {
                    Error = "XSRF token validation failed"
                });
                return;
            }

            await next(ctx);
        });
    }
}
