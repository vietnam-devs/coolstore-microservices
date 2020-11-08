using System;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using N8T.Infrastructure.App.Dtos;
using N8T.Infrastructure.App.Requests.Identity;
using SaleService.Domain.Gateway;

namespace SaleService.Infrastructure.Gateway
{
    public class UserGateway : IUserGateway
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<UserGateway> _logger;

        public UserGateway(DaprClient daprClient, ILogger<UserGateway> logger)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UserDto> GetUserInfo(string userId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("{Prefix}: GetUserInfo by id={Id}", nameof(UserGateway), userId);

            var requestData = new UserByIdRequest {UserId = userId};
            return await _daprClient.InvokeMethodAsync<UserByIdRequest, UserDto>(
                "identityapp", "get-user-by-id", requestData, cancellationToken: cancellationToken);
        }
    }
}
