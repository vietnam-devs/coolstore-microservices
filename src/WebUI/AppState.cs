using System;
using System.Threading.Tasks;
using WebUI.Model;
using WebUI.Services;

namespace WebUI
{
  public class AppState
  {
    private readonly JsInterop _jsInterop;

    public AppState(JsInterop jsInterop)
    {
      _jsInterop = jsInterop;
      User = new UserModel();
    }

    public bool IsSignedIn => User?.AccessToken != null;

    public UserModel User { get; private set; }

    public async Task InitAppStore(Action doAction = null)
    {
      var user = await _jsInterop.GetUser();
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
  }
}
