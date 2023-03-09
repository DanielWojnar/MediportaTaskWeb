using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MediportaTaskWeb.Data.Http
{
    public class HttpClientService : IHttpClientService
    {
        private static HttpClient client = new HttpClient();
        private readonly ILogger<HttpClientService> _logger;
        public HttpClientService(ILogger<HttpClientService> logger)
        {
            _logger = logger;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<T?> GetAsync<T>(string uri, bool gZipDecompress = false) where T : class
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                Stream contentSteam = await response.Content.ReadAsStreamAsync();
                if (gZipDecompress)
                {
                    contentSteam = new GZipStream(contentSteam, CompressionMode.Decompress);
                }

                if (!response.IsSuccessStatusCode)
                {
                    string contentString;
                    using (var sr = new StreamReader(contentSteam))
                    {
                        contentString = sr.ReadToEnd();
                    }
                    _logger.LogWarning($"Get request to {uri} failed. Response status code: {response.StatusCode}. Response body as string: {contentString}");
                    return null;
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                T? dto = await JsonSerializer.DeserializeAsync<T>(contentSteam, options);
                return dto;
            }
            catch (Exception e)
            {
                _logger.LogError($"Get request to {uri} failed. Exception: {e.Message}");
                return null;
            }
        }
    }
}
