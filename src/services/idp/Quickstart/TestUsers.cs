// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer4.Quickstart.UI
{
    public class TestUsers
    {
        public static List<TestUser> Users = new List<TestUser>
        {
            new TestUser{SubjectId = "4696cdd0-d20d-414b-8cf0-4d272def8861", Username = "phi", Password = "123", 
                Claims = 
                {
                    new Claim(JwtClaimTypes.Name, "Phi Huynh"),
                    new Claim(JwtClaimTypes.GivenName, "Phi"),
                    new Claim(JwtClaimTypes.FamilyName, "Huynh"),
                    new Claim(JwtClaimTypes.Email, "phi.huynh@nashtechglobal.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://nashtechglobal.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }
            },
            new TestUser{SubjectId = "51db5230-ef16-4e5f-ad5f-1820c2cc7a99", Username = "phuong", Password = "123", 
                Claims = 
                {
                    new Claim(JwtClaimTypes.Name, "Phuong Le"),
                    new Claim(JwtClaimTypes.GivenName, "Phuong"),
                    new Claim(JwtClaimTypes.FamilyName, "Le"),
                    new Claim(JwtClaimTypes.Email, "phuong.le@nashtechglobal.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://nashtechglobal.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }
            },
            new TestUser{SubjectId = "C025822B-74D9-4899-98B0-DAF1EF0D5D6E", Username = "thang", Password = "123",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Thang Chung"),
                    new Claim(JwtClaimTypes.GivenName, "Thang"),
                    new Claim(JwtClaimTypes.FamilyName, "Chung"),
                    new Claim(JwtClaimTypes.Email, "thang.chung@nashtechglobal.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://nashtechglobal.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }
            }
        };
    }
}
