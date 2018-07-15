using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace VND.FW.Infrastructure.AspNetCore
{
    public class RestClient
    {
        private readonly HttpClient _client;
        private IEnumerable<KeyValuePair<string, string>> _openTracingInfo;
				private readonly ILogger<RestClient> _logger;

				public RestClient(ILoggerFactory logger, HttpClient client = null)
        {
            _client = client ?? new HttpClient();
						_logger = logger.CreateLogger<RestClient>();
        }

        public void SetOpenTracingInfo(IEnumerable<KeyValuePair<string, string>> info)
        {
            _openTracingInfo = info;
        }

        public async Task<TReturnMessage> GetAsync<TReturnMessage>(string serviceUrl)
            where TReturnMessage : class, new()
        {
            var enrichClient = EnrichHeaderInfo(_client, _openTracingInfo);

            var response = await enrichClient.GetAsync(serviceUrl);
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode) return await Task.FromResult(new TReturnMessage());
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TReturnMessage>(result);
        }

        public async Task<TReturnMessage> PostAsync<TReturnMessage>(string serviceUrl, object dataObject = null)
            where TReturnMessage : class, new()
        {
            var enrichClient = EnrichHeaderInfo(_client, _openTracingInfo);
            var content = dataObject != null ? JsonConvert.SerializeObject(dataObject) : "{}";

            var response = await _client.PostAsync(serviceUrl, GetStringContent(content));
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode) return await Task.FromResult(new TReturnMessage());
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TReturnMessage>(result);
        }

        public async Task<TReturnMessage> PutAsync<TReturnMessage>(string serviceUrl, object dataObject = null)
            where TReturnMessage : class, new()
        {
            var enrichClient = EnrichHeaderInfo(_client, _openTracingInfo);
            var content = dataObject != null ? JsonConvert.SerializeObject(dataObject) : "{}";

            var response = await _client.PutAsync(serviceUrl, GetStringContent(content));
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode) return await Task.FromResult(new TReturnMessage());
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TReturnMessage>(result);
        }

        public async Task<bool> DeleteAsync(string serviceUrl)
        {
            var enrichClient = EnrichHeaderInfo(_client, _openTracingInfo);

            var response = await _client.DeleteAsync(serviceUrl);
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode) return await Task.FromResult(false);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(result);
        }

        private static StringContent GetStringContent(string content)
        {
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        private static HttpClient EnrichHeaderInfo(
            HttpClient client,
            IEnumerable<KeyValuePair<string, string>> openTracingInfo)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var info in openTracingInfo) client.DefaultRequestHeaders.Add(info.Key, info.Value);

            return client;
        }
    }
}