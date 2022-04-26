// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Microsoft.Extensions.Configuration;
using Rsk.TokenExchange;
using Rsk.TokenExchange.Validators;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("sale.all"),
                new ApiScope("sale.read"),
                new ApiScope("sale.write"),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("sale-api", "Sale APIs")
                {
                    Scopes = { "sale.all", "sale.read", "sale.write" }
                }
            };

        public static IEnumerable<Client> Clients(IConfiguration config)
        {
            var publicClientUrl = config.GetValue("PublicClientUrl", "https://web.cs.local:5000");
            var internalClientUrl = config.GetValue("InternalClientUrl", "https://localhost:5000");
            
            return new Client[]
            {
                // BFF gateway
                new Client
                {
                    ClientId = "gw-api",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,

                    RedirectUris = { $"{internalClientUrl}/signin-oidc", $"{publicClientUrl}/signin-oidc" },

                    BackChannelLogoutUri = $"{internalClientUrl}/logout",

                    PostLogoutRedirectUris = { $"{internalClientUrl}/signout-callback-oidc", $"{internalClientUrl}/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "sale.all" }
                },
                new Client
                {
                    ClientId = "sale-api",
                    ClientSecrets = new[] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = new[] {"urn:ietf:params:oauth:grant-type:token-exchange"},
                    AllowedScopes = new[] { "sale.read", "sale.write" }
                }
            };
        }

        //public static IEnumerable<Client> Clients =>
        //    new Client[]
        //    {
        //        // BFF gateway
        //        new Client
        //        {
        //            ClientId = "gw-api",
        //            ClientSecrets = { new Secret("secret".Sha256()) },

        //            AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,

        //            RedirectUris = { "https://web.cs.local:5000/signin-oidc", "https://localhost:5000/signin-oidc" },

        //            BackChannelLogoutUri = "https://web.cs.local:5000/logout",

        //            PostLogoutRedirectUris = { "https://web.cs.local:5000/signout-callback-oidc", "https://localhost:5000/signout-callback-oidc" },

        //            AllowOfflineAccess = true,
        //            AllowedScopes = { "openid", "profile", "sale.all" }
        //        },
        //        new Client
        //        {
        //            ClientId = "sale-api",
        //            ClientSecrets = new[] {new Secret("secret".Sha256())},
        //            AllowedGrantTypes = new[] {"urn:ietf:params:oauth:grant-type:token-exchange"},
        //            AllowedScopes = new[] { "sale.read", "sale.write" }
        //        }
        //    };
    }

    public class CustomTokenExchangeRequestValidator : ITokenExchangeRequestValidator
    {
        private readonly ISubjectTokenValidator _subjectTokenValidator;

        public CustomTokenExchangeRequestValidator(ISubjectTokenValidator subjectTokenValidator)
        {
            _subjectTokenValidator = subjectTokenValidator ?? throw new ArgumentNullException(nameof(subjectTokenValidator));
        }

        public async Task<ITokenExchangeValidationResult> Validate(ITokenExchangeRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            // validate subject token is issued by IdentityServer
            var result = await _subjectTokenValidator.Validate(request.SubjectToken, request.SubjectTokenType);

            // validate that the requester is an intended audience of the subject token
            var audiences = result.Claims.Where(x => x.Type == "aud").ToList();
            var clientId = result.Claims.FirstOrDefault(x => x.Type == "client_id");

            if (audiences.All(x => x.Value != request.ClientId) && clientId?.Value != request.ClientId)
            {
                return TokenExchangeValidationResult.Failure("Requester must be a recipient of the subject token");
            }

            return result.IsValid
                ? TokenExchangeValidationResult.Success(result.Claims)
                : TokenExchangeValidationResult.Failure("Invalid subject token");
        }
    }
}
