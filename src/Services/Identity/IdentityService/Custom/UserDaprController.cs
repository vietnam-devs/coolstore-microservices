using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using N8T.Infrastructure.App.Dtos;
using N8T.Infrastructure.App.Requests.Identity;

namespace IdentityService.Custom
{
    [ApiController]
    public class UserDaprController : ControllerBase
    {
        [HttpPost("/get-user-by-id")]
        public async Task<UserDto> GetById(UserByIdRequest requestData)
        {
            var result = GetUsers().FirstOrDefault(x => x.Id == requestData.UserId);

            return await Task.FromResult(result);
        }

        private IEnumerable<UserDto> GetUsers()
        {
            return new List<UserDto>
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
        }
    }
}
