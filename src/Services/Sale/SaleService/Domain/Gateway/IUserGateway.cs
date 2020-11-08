using System.Threading;
using System.Threading.Tasks;
using N8T.Infrastructure.App.Dtos;

namespace SaleService.Domain.Gateway
{
    public interface IUserGateway
    {
        Task<UserDto> GetUserInfo(string userId, CancellationToken cancellationToken = default);
    }
}
