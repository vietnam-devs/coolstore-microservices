using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace WebUI.Services
{
  public static class JsInterop
  {
    public static async Task WriteLog(string message)
    {
      await JSRuntime.Current.InvokeAsync<bool>("log", message);
    }
  }
}
