using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Gateway.Middleware;

/// <summary>
/// https://github.com/DuendeSoftware/BFF/blob/36891c9129/src/Duende.Bff/Endpoints/BffAuthorizationMiddlewareResultHandler.cs
/// </summary>
public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _handler;

    public CustomAuthorizationMiddlewareResultHandler()
    {
        _handler = new AuthorizationMiddlewareResultHandler();
    }
    
    public Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Challenged)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        }

        if (!authorizeResult.Forbidden) return _handler.HandleAsync(next, context, policy, authorizeResult);
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        
        return Task.CompletedTask;

    }
}