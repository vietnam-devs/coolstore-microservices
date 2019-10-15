using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace VND.CoolStore.AccessControl.Customized
{
    public class CustomizedProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = context.Subject.Claims.ToList();
            string userId = claims.FirstOrDefault(x => x.Type == "sub").Value;

            // only for demo
            if (userId == "C025822B-74D9-4899-98B0-DAF1EF0D5D6E")
            {
                context.IssuedClaims.Add(new Claim("role", "user"));
                context.IssuedClaims.Add(new Claim(JwtClaimTypes.Name, "Bob Smith"));
            }

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
