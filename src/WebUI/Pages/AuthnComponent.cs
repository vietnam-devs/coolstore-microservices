using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Components;
using WebUI.Services;

namespace WebUI.Pages
{
  public class AuthnComponent : BlazorComponent
  {
    [Inject] public AuthnService AuthzService { get; set; }
    [Inject] public AppState AppState { get; set; }

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
