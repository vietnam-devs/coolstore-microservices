using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Components;
using WebUI.Services;

namespace WebUI.Features
{
  public class AuthnComponent : BaseComponent
  {
    [Inject] public AuthnService AuthzService { get; set; }

    protected override async Task OnInitAsync()
    {
      await EnsureAuthn();
    }

    public async Task EnsureAuthn()
    {
      await AppState.InitAppStore(async () => await AuthzService.EnsureAuthz(AppState.User));
    }
  }
}
