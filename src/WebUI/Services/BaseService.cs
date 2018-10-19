using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebUI.Model;

namespace WebUI.Services
{
  public abstract class BaseService
  {
    public AppState AppState { get; }
    public HttpClient RestClient { get; }
    public ConfigModel Config { get; }

    protected BaseService(AppState appState, ConfigModel config, HttpClient httpClient)
    {
      AppState = appState;
      RestClient = httpClient;
      Config = config;

      RestClient.DefaultRequestHeaders.Accept.Clear();
      RestClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public Task SetHeaderToken()
    {
      RestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppState.GetToken());
      return Task.CompletedTask;
    }
  }
}
