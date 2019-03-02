using System;
using System.Threading.Tasks;
using WebUI.Model;
using WebUI.Services;
using WebUI.Shared.Sidebar;

namespace WebUI
{
  public class AppState
  {
    private readonly JsInteropService _jsInteropService;

    public AppState(JsInteropService jsInteropService)
    {
      _jsInteropService = jsInteropService;
    }

    public bool IsSignedIn => User?.AccessToken != null;

    public UserModel User { get; private set; } = new UserModel();
    public CartModel Cart { get; set; } = new CartModel();
    public SideBarViewModel SideBar { get; set; } = new SideBarViewModel();
    public Pagination<ItemModel> ItemPagination { get; set; } = new Pagination<ItemModel>();

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

    public async Task LogInfoOut(string info)
    {
      await _jsInteropService.WriteLog(info);
    }

    public string GetToken()
    {
      return User.AccessToken;
    }

    public Guid? GetCurrentCart()
    {
      return Cart.Id;
    }
  }
}
