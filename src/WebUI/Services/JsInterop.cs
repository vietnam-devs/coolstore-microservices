using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace WebUI.Services
{
  public static class JsInterop
  {
    public static async Task WriteLog(string message)
    {
      await JSRuntime.Current.InvokeAsync<bool>("logToConsole", message);
    }

    public static async Task<UserModel> GetUser()
    {
      return await JSRuntime.Current.InvokeAsync<UserModel>("getUser");
    }

    public static async Task StartSignin()
    {
      await JSRuntime.Current.InvokeAsync<bool>("startSignin");
    }

    public static async Task Callback()
    {
      await JSRuntime.Current.InvokeAsync<bool>("callback");
    }

    public static async Task Silent()
    {
      await JSRuntime.Current.InvokeAsync<bool>("silent");
    }
  }

  public class UserModel
  {
    public string AccessToken { get; set; }
    public string TokenType { get; set; }
    public string Scope { get; set; }
    public ProfileModel Profile { get; set; } = new ProfileModel();
  }

  public class ProfileModel
  {
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
  }
}
