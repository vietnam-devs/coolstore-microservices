// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

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
						}
					}
				}
			};
		}

		public static IEnumerable<Client> GetClients() => new[]
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
						"http://localhost:5000/swagger"
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
						"review_api_scope"
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
					ClientUri = "http://identityserver.io",

					AllowedGrantTypes = GrantTypes.Implicit,
					AllowAccessTokensViaBrowser = true,

					RedirectUris =
					{
						"http://localhost:5002/index.html",
						"http://localhost:5002/callback.html",
						"http://localhost:5002/silent.html",
						"http://localhost:5002/popup.html",
					},

					PostLogoutRedirectUris = { "http://localhost:5002/index.html" },
					AllowedCorsOrigins = { "http://localhost:5002" },

					AllowedScopes = { "openid", "profile", "api1" }
				}
			};
	}
}
