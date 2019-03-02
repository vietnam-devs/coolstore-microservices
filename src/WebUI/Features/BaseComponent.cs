using Microsoft.AspNetCore.Blazor.Components;
using WebUI.Services;

namespace WebUI.Features
{
  public class BaseComponent : BlazorComponent
  {
    [Inject] public JsInteropService JsInteropService { get; set; }
    [Inject] public AppState AppState { get; set; }
  }
}
