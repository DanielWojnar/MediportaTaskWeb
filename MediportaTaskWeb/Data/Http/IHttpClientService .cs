namespace MediportaTaskWeb.Data.Http
{
    public interface IHttpClientService
    {
        public Task<T?> GetAsync<T>(string uri, bool gZipDecompress = false) where T : class;
    }
}
