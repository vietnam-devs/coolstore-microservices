// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace Idp
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
					Name = "inventory_api",
					DisplayName = "Inventory API",
					Scopes =
					{
						new Scope
						{
							Name = "inventory_api_scope",
							Description = "inventory_api_scope"
						}
					}
				}
			};
		}

		public static IEnumerable<Client> GetClients()
		{
			return new[]
			{
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
				},

                // Inventory Swagger UI
                new Client
				{
					ClientId = "inventory_swagger_id",
					ClientName = "inventory_swagger_app",
					ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
					AllowedGrantTypes = GrantTypes.Implicit,
					AllowAccessTokensViaBrowser = true,
					RedirectUris =
					{
						"http://localhost:51032/swagger/oauth2-redirect.html",
						"https://192.168.169.12:8443/inventory/swagger/oauth2-redirect.html",
						"https://192.168.1.5:8443/inventory/swagger/oauth2-redirect.html"
					},
					PostLogoutRedirectUris =
					{
						"http://localhost:51032/swagger",
						"https://192.168.169.12:8443/inventory/swagger",
						"https://192.168.1.5:8443/inventory/swagger"
					},
					AllowedCorsOrigins =
					{
						"http://localhost:51032",
						"https://192.168.169.12:8443",
						"https://192.168.1.5:8443"
					},
					AccessTokenLifetime = 300,
					AllowedScopes =
					{
						"inventory_api_scope"
					}
				}
			};
		}
	}
}
