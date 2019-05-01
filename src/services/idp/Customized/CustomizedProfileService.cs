using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace VND.CoolStore.Services.Idp.Customized
{
    public class CustomizedProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = context.Subject.Claims.ToList();
            string userId = claims.FirstOrDefault(x => x.Type == "sub").Value;

            // TODO: hard code only for demo
            if (userId == "4696cdd0-d20d-414b-8cf0-4d272def8861")
            {
                context.IssuedClaims.Add(new Claim("role", "admin"));
                context.IssuedClaims.Add(new Claim("username", "phi"));
            }
            else if (userId == "C025822B-74D9-4899-98B0-DAF1EF0D5D6E")
            {
                context.IssuedClaims.Add(new Claim("role", "tester"));
                context.IssuedClaims.Add(new Claim("username", "thang"));
            }
            else
            {
                context.IssuedClaims.Add(new Claim("role", "user"));
                context.IssuedClaims.Add(new Claim("username", "phuong"));
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
