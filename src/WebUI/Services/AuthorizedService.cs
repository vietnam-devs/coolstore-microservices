using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Services;
using WebUI.Model;

namespace WebUI.Services
{
  public class AuthorizedService
  {
    private readonly IUriHelper _uriHelper;

    public AuthorizedService(IUriHelper uriHelper)
    {
      _uriHelper = uriHelper;
    }

    public Task EnsureAuthz(UserModel userState)
    {
      if (userState?.AccessToken != null && userState.Profile != null)
      {
        return Task.CompletedTask;
      }
      _uriHelper.NavigateTo("/unauthorized");
      return Task.CompletedTask;
    }
  }
}
