// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityServer4
{
  public static class Config
  {
    public static IEnumerable<IdentityResource> GetIdentityResources() => new IdentityResource[]
    {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
    };

    public static IEnumerable<ApiResource> GetApis() => new[]
    {
            new ApiResource
            {
                Name = "api",
                DisplayName = "API",
                Scopes =
                {
                    new Scope
                    {
                        Name = "inventory_api_scope",
                        Description = "Inventory API scope"
                    },
                    new Scope
                    {
                        Name = "cart_api_scope",
                        Description = "Cart API Scope"
                    },
                    new Scope
                    {
                        Name = "catalog_api_scope",
                        Description = "Catalog API Scope"
                    },
                    new Scope
                    {
                        Name = "rating_api_scope",
                        Description = "Rating API Scope"
                    }
                }
            }
        };

    public static IEnumerable<Client> GetDevClients() => new[]
    {
            // Inventory Swagger UI
            new Client
            {
                ClientId = "swagger_id",
                ClientName = "swagger_app",
                ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris =
                {
                    "http://localhost:5002/swagger/oauth2-redirect.html",
                    "http://localhost:5003/swagger/oauth2-redirect.html",
                    "http://localhost:5004/swagger/oauth2-redirect.html",
                    "http://localhost:5005/swagger/oauth2-redirect.html",
                    "http://localhost:5006/swagger/oauth2-redirect.html",
                    "http://localhost:5007/swagger/oauth2-redirect.html",
                    "http://localhost:8082/swagger/oauth2-redirect.html",
                },
                PostLogoutRedirectUris =
                {
                    "http://localhost:5001/signout-oidc"
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:5002",
                    "http://localhost:5003",
                    "http://localhost:5004",
                    "http://localhost:5005",
                    "http://localhost:5006",
                    "http://localhost:5007",
                    "http://localhost:8082",
                },
                AccessTokenLifetime = 300,
                AllowedScopes =
                {
                    "inventory_api_scope",
                    "cart_api_scope",
                    "catalog_api_scope",
                    "rating_api_scope"
                }
            },
            // SPA client using implicit flow
            new Client
            {
                ClientId = "spa",
                ClientName = "SPA Client",
                ClientUri = "http://localhost:8080",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris =
                {
                    "http://localhost:8080/callback"
                },

                PostLogoutRedirectUris =
                {
                    "http://localhost:8080/"
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:8080"
                },

                AllowedScopes =
                {
                    "inventory_api_scope",
                    "cart_api_scope",
                    "catalog_api_scope",
                    "rating_api_scope",
                    "openid",
                    "profile"
                }
            },
            new Client // graphql
            {
                ClientId = "graphql",
                ClientName = "graphql client",
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
                AccessTokenLifetime = 3600,

                RedirectUris =
                {
                    "https://www.getpostman.com/oauth2/callback",
                    "http://localhost:5011"
                },

                PostLogoutRedirectUris =
                {
                    "https://www.getpostman.com/oauth2/callback/",
                    "http://localhost:5011"
                },
                AllowedCorsOrigins =
                {
                    "https://www.getpostman.com",
                    "http://localhost:5011"
                },

                AllowedScopes =
                {
                    "inventory_api_scope",
                    "cart_api_scope",
                    "catalog_api_scope",
                    "rating_api_scope",
                    "openid",
                    "profile"
                }
            },
            new Client // backoffice
            {
                ClientId = "backoffice",
                ClientName = "backoffice Client",
                ClientUri = "http://localhost:3000",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris =
                {
                    "http://localhost:3000/signin-callback.html",
                    "http://localhost:3000/silent-renew.html"
                },

                PostLogoutRedirectUris =
                {
                    "http://localhost:3000"
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:3000"
                },

                AllowedScopes =
                {
                    "inventory_api_scope",
                    "cart_api_scope",
                    "catalog_api_scope",
                    "rating_api_scope",
                    "openid",
                    "profile"
                }
            }
        };

    public static IEnumerable<Client> GetClients(IConfigurationSection hostSettings) => new[]
    {
            // Inventory Swagger UI
            new Client
            {
                ClientId = "swagger_id",
                ClientName = "swagger_app",
                ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris =
                {
                    $"{hostSettings.GetValue<string>("SwaggerAllowedCorsOrigin")}/catalog/swagger/oauth2-redirect.html",
                    $"{hostSettings.GetValue<string>("SwaggerAllowedCorsOrigin")}/cart/swagger/oauth2-redirect.html",
                    $"{hostSettings.GetValue<string>("SwaggerAllowedCorsOrigin")}/inventory/swagger/oauth2-redirect.html",
                    $"{hostSettings.GetValue<string>("SwaggerAllowedCorsOrigin")}/price/swagger/oauth2-redirect.html",
                    $"{hostSettings.GetValue<string>("SwaggerAllowedCorsOrigin")}/rating/swagger/oauth2-redirect.html",
                },
                PostLogoutRedirectUris =
                {
                    "http://localhost:5001/signout-oidc"
                },
                AllowedCorsOrigins =
                {
                    hostSettings.GetValue<string>("SwaggerAllowedCorsOrigin")
                },
                AccessTokenLifetime = 300,
                AllowedScopes =
                {
                    "inventory_api_scope",
                    "cart_api_scope",
                    "catalog_api_scope",
                    "rating_api_scope"
                }
            },
            new Client // graphql
            {
                ClientId = "graphql",
                ClientName = "graphql client",
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
                AccessTokenLifetime = 3600,

                RedirectUris =
                {
                    "https://www.getpostman.com/oauth2/callback",
                    "http://localhost:5011"
                },

                PostLogoutRedirectUris =
                {
                    "https://www.getpostman.com/oauth2/callback/",
                    "http://localhost:5011"
                },
                AllowedCorsOrigins =
                {
                    "https://www.getpostman.com",
                    "http://localhost:5011"
                },

                AllowedScopes =
                {
                    "inventory_api_scope",
                    "cart_api_scope",
                    "catalog_api_scope",
                    "rating_api_scope",
                    "openid",
                    "profile"
                }
            },
            new Client // SPA client using implicit flow
            {
                ClientId = "spa",
                ClientName = "SPA Client",
                ClientUri = hostSettings.GetValue<string>("SpaAllowedCorsOrigin"),
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris =
                {
                    $"{hostSettings.GetValue<string>("SpaAllowedCorsOrigin")}/callback",
                },

                PostLogoutRedirectUris =
                {
                    $"{hostSettings.GetValue<string>("SpaAllowedCorsOrigin")}/"
                },
                AllowedCorsOrigins =
                {
                    hostSettings.GetValue<string>("SpaAllowedCorsOrigin"),
                },

                AllowedScopes =
                {
                    "inventory_api_scope",
                    "cart_api_scope",
                    "catalog_api_scope",
                    "rating_api_scope",
                    "openid",
                    "profile"
                }
            },
            new Client // backoffice
            {
                ClientId = "backoffice",
                ClientName = "backoffice Client",
                ClientUri = hostSettings.GetValue<string>("BackOfficeAllowedCorsOrigin"),
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris =
                {
                    $"{hostSettings.GetValue<string>("BackOfficeAllowedCorsOrigin")}/signin-callback.html",
                    $"{hostSettings.GetValue<string>("BackOfficeAllowedCorsOrigin")}/silent-renew.html"
                },

                PostLogoutRedirectUris =
                {
                    $"{hostSettings.GetValue<string>("BackOfficeAllowedCorsOrigin")}/"
                },
                AllowedCorsOrigins =
                {
                    hostSettings.GetValue<string>("BackOfficeAllowedCorsOrigin")
                },

                AllowedScopes =
                {
                    "inventory_api_scope",
                    "cart_api_scope",
                    "catalog_api_scope",
                    "rating_api_scope",
                    "openid",
                    "profile"
                }
            }
        };
  }
}
