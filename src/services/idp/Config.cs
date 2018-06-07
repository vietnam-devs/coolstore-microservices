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
			new ApiResource("api1", "My API #1"),
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
                        "http://localhost:51033/swagger/oauth2-redirect.html",
						"https://localhost:8443/inventory/swagger/oauth2-redirect.html"
					},
					PostLogoutRedirectUris =
					{
						"http://localhost:51033/swagger",
						"https://localhost:8443/inventory/swagger"
					},
					AllowedCorsOrigins =
					{
						"http://localhost:51033",
						"https://localhost:8443"
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
