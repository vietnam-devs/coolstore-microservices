using System;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace N8T.Infrastructure.Auth
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomAuth<TType>(this IServiceCollection services,
            IConfiguration config,
            Action<JwtBearerOptions> configureOptions = null,
            Action<IServiceCollection> configureMoreActions = null)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    config.Bind("Auth", options);
                    configureOptions?.Invoke(options);
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.Name);
                });
            });

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthBehavior<,>));

            services.Scan(s => s
                .FromAssemblyOf<TType>()
                .AddClasses(c => c
                    .AssignableTo<IAuthorizationHandler>()).As<IAuthorizationHandler>()
                .AddClasses(c => c
                    .AssignableTo<IAuthorizationRequirement>()).As<IAuthorizationRequirement>()
            );

            configureMoreActions?.Invoke(services);

            return services;
        }
    }
}
