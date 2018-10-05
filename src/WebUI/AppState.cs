using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Services;
using WebUI.Model;
using WebUI.Services;

namespace WebUI
{
  public class AppState
  {
    private readonly JsInteropService _jsInteropService;
    private readonly IUriHelper _uriHelper;

    public AppState(JsInteropService jsInteropService, IUriHelper uriHelper)
    {
      _jsInteropService = jsInteropService;
      _uriHelper = uriHelper;
      User = new UserModel();
    }

    public bool IsSignedIn => User?.AccessToken != null;

    public UserModel User { get; private set; }
    public SideBarModel SideBar { get; set; }

    public async Task InitAppStore(Action doAction = null)
    {
      var user = await _jsInteropService.GetUser();
      if (user != null)
      {
        await UpdateUser(user);
        doAction?.Invoke();
      }
    }

    public Task UpdateUser(UserModel user, Action doAction = null)
    {
      User = user;
      doAction?.Invoke();
      return Task.CompletedTask;
    }

    public Task Redirect(string path = "/")
    {
      _uriHelper.NavigateTo(path);
      return Task.CompletedTask;
    }
  }
}
