using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Services;
using Microsoft.JSInterop;
using WebUI.Model;

namespace WebUI.Services
{
  public class JsInteropService
  {
    public JsInteropService(IUriHelper uriHelper)
    {
      UriHelper = uriHelper;
    }

    public IUriHelper UriHelper { get; }

    public async Task WriteLog(string message)
    {
      await JSRuntime.Current.InvokeAsync<bool>("logToConsole", message);
    }

    public async Task<UserModel> GetUser()
    {
      return await JSRuntime.Current.InvokeAsync<UserModel>("getUser");
    }

    public async Task StartSignin()
    {
      await JSRuntime.Current.InvokeAsync<bool>("startSignin");
    }

    public async Task StartSignOut(string idToken)
    {
      await JSRuntime.Current.InvokeAsync<bool>("startSignOut", idToken);
    }

    public async Task<UserModel> Callback()
    {
      return await JSRuntime.Current.InvokeAsync<UserModel>("callback");
    }

    public async Task Silent()
    {
      await JSRuntime.Current.InvokeAsync<bool>("silent");
    }

    public async Task Navigate(string uri = "/")
    {
      UriHelper.NavigateTo(uri);
      //await JSRuntime.Current.InvokeAsync<bool>("refresh", uri);
      await Task.CompletedTask;
    }
  }
}
