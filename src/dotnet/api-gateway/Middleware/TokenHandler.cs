using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Middleware;

public static class TokenHandler
{
    public static void HandleToken(TokenValidatedContext context)
    {
        if (context.TokenEndpointResponse == null)
        {
            throw new Exception("TokenEndpointResponse expected!");
        }

        var accessToken = context.TokenEndpointResponse.AccessToken;
        var idToken = context.TokenEndpointResponse.IdToken;
        var refreshToken = context.TokenEndpointResponse.RefreshToken;
        var expiresIn = context.TokenEndpointResponse.ExpiresIn;
        var expiresAt = new DateTimeOffset(DateTime.Now).AddSeconds(Convert.ToInt32(expiresIn));

        context.HttpContext.Session.SetString(SessionKeys.AccessToken, accessToken);
        context.HttpContext.Session.SetString(SessionKeys.IdToken, idToken);
        context.HttpContext.Session.SetString(SessionKeys.RefreshToken, refreshToken);
        context.HttpContext.Session.SetString(SessionKeys.ExpiresAt, "" + expiresAt.ToUnixTimeSeconds());
    }
}