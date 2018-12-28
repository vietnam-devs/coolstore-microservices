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
            Name = "pricing_api_scope",
            Description = "Pricing API Scope"
          },
          new Scope
          {
            Name = "review_api_scope",
            Description = "Review API Scope"
          },
          new Scope
          {
            Name = "catalog_api_scope",
            Description="Catalog API Scope"
          },
          new Scope
          {
            Name = "rating_api_scope",
            Description="Rating API Scope"
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
          "http://localhost:5007"
        },
        AccessTokenLifetime = 300,
        AllowedScopes =
        {
          "inventory_api_scope",
          "cart_api_scope",
          "pricing_api_scope",
          "review_api_scope",
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
          "http://localhost:8080/callback",
		  $"{hostSettings.GetValue<string>("SpaAllowedCorsOrigin")}/callback",
        },

        PostLogoutRedirectUris =
        {
          "http://localhost:8080/"
        },
        AllowedCorsOrigins =
        {
          "http://localhost:8080"
        },

        AllowedScopes = {
          "inventory_api_scope",
          "cart_api_scope",
          "pricing_api_scope",
          "review_api_scope",
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
          $"{hostSettings.GetValue<string>("SwaggerAllowedCorsOrigin")}/review/swagger/oauth2-redirect.html",
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
          "pricing_api_scope",
          "review_api_scope",
          "catalog_api_scope",
          "rating_api_scope"
        }
      },
			// SPA client using implicit flow
			new Client
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

        AllowedScopes = {
          "inventory_api_scope",
          "cart_api_scope",
          "pricing_api_scope",
          "review_api_scope",
          "catalog_api_scope",
          "rating_api_scope",
          "openid",
          "profile"
        }
      }
    };
  }
}
