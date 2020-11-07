using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using N8T.Infrastructure.App.Dtos;

namespace IdentityService.Custom
{
    [ApiController]
    public class UserDaprController : ControllerBase
    {
        [HttpPost("/get-users")]
        public async Task<IEnumerable<UserDto>> GetByIds()
        {
            var result = new List<UserDto>
            {
                new UserDto
                {
                    Id = "88421113",
                    FullName = "Bob Smith",
                    Email = "BobSmith@email.com",
                    Address = "One Hacker Way, Heidelberg, 69118, Germany"
                },
                new UserDto
                {
                    Id = "818727",
                    FullName = "Alice Smith",
                    Email = "AliceSmith@email.com",
                    Address = "One Hacker Way, Heidelberg, 69118, Germany"
                }
            };

            return await Task.FromResult(result);
        }
    }
}
