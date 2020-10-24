// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityService
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[] {new IdentityResources.OpenId(), new IdentityResources.Profile(),};

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[] {new ApiScope("scope1"), new ApiScope("scope2"),};

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256())},
                    AllowedScopes = {"scope1"}
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = {new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"https://localhost:44300/signin-oidc"},
                    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    PostLogoutRedirectUris = {"https://localhost:44300/signout-callback-oidc"},
                    AllowOfflineAccess = true,
                    AllowedScopes = {"openid", "profile", "scope2"}
                },

                // password flow
                new Client
                {
                    ClientId = "coolstore.password",
                    ClientSecrets = {new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = {"openid", "profile", "scope1", "scope2"}
                },

                // web
                new Client
                {
                    ClientId = "coolstore.web",
                    ClientName = "React Web",
                    ClientUri = "http://localhost:3000",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris =
                    {
                        "http://localhost:3000/auth/callback",
                        "http://localhost:3000/auth/silent-renew"
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:3000"
                    },
                    AllowedCorsOrigins =
                    {
                        "http://localhost:3000"
                    },
                    AllowedScopes = { "openid", "profile", "scope2" }
                },

                // code flow
                new Client
                {
                    ClientId = "store.code",
                    ClientSecrets = {new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = {"https://www.getpostman.com/oauth2/callback", "https://app.getpostman.com/oauth2/callback"},
                    PostLogoutRedirectUris = {"https://app.getpostman.com", "https://www.getpostman.com"},

                    AllowedScopes = {"openid", "profile", "scope1", "scope2"}
                },

                // password flow
                new Client
                {
                    ClientId = "store.password",
                    ClientSecrets = {new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = {"openid", "profile", "scope1", "scope2"}
                }
            };
    }
}
