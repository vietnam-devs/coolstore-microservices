// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace IdentityServer4
{
  public static class Config
  {
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
      return new IdentityResource[]
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
      };
    }

    public static IEnumerable<ApiResource> GetApis()
    {
      return new ApiResource[]
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
    }

    public static IEnumerable<Client> GetClients(bool isDevelopment, IConfigurationSection hostSettings) => new[]
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
          "http://localhost:5000/swagger/oauth2-redirect.html"
        },
        PostLogoutRedirectUris =
        {
          "http://localhost:5001/signout-oidc"
        },
        AllowedCorsOrigins =
        {
          "http://localhost:5000"
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
			// MVC client using hybrid flow
			new Client
      {
        ClientId = "mvc",
        ClientName = "MVC Client",

        AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
        ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

        RedirectUris = { "https://localhost:5001/signin-oidc" },
        FrontChannelLogoutUri = "https://localhost:5001/signout-oidc",
        PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

        AllowOfflineAccess = true,
        AllowedScopes = { "openid", "profile", "inventory_api" }
      },
			// SPA client using implicit flow
			new Client
      {
        ClientId = "spa",
        ClientName = "SPA Client",
        ClientUri = isDevelopment
          ? "http://localhost:8080"
          : hostSettings.GetValue<string>("SwaggerAllowedCorsOrigin"),
        AllowedGrantTypes = GrantTypes.Implicit,
        AllowAccessTokensViaBrowser = true,

        RedirectUris =
        {
          "http://localhost:8080/callback",
          $"{hostSettings.GetValue<string>("SwaggerAllowedCorsOrigin")}/callback"
        },

        PostLogoutRedirectUris =
        {
          "http://localhost:8080/",
          $"{hostSettings.GetValue<string>("SwaggerAllowedCorsOrigin")}/"
        },
        AllowedCorsOrigins =
        {
          isDevelopment
          ? "http://localhost:8080"
          : hostSettings.GetValue<string>("SwaggerAllowedCorsOrigin"),
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
