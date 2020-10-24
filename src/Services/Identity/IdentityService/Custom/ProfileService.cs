using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace IdentityService.Custom
{
    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.GetSubjectId();

            switch (userId)
            {
                case "88421113": // bob
                    context.IssuedClaims.Add(new Claim("user_role", "sys_admin"));
                    break;
                default:
                    context.IssuedClaims.Add(new Claim("user_role", "normal_user"));
                    break;
            }

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            context.IsActive = sub != null;

            return Task.CompletedTask;
        }
    }
}
