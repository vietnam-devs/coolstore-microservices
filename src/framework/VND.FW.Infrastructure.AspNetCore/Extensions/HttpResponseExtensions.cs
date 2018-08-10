using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace VND.Fw.Infrastructure.AspNetCore.Extensions
{
  public static class HttpResponseExtensions
  {
    private static readonly JsonSerializer Serializer = new JsonSerializer
    {
      NullValueHandling = NullValueHandling.Ignore
    };

    public static void WriteJson<T>(this HttpResponse response, T obj, string contentType = null)
    {
      response.ContentType = contentType ?? "application/json";
      using (var writer = new HttpResponseStreamWriter(response.Body, Encoding.UTF8))
      {
        using (var jsonWriter = new JsonTextWriter(writer))
        {
          jsonWriter.CloseOutput = false;
          jsonWriter.AutoCompleteOnClose = false;

          Serializer.Serialize(jsonWriter, obj);
        }
      }
    }
  }
}
