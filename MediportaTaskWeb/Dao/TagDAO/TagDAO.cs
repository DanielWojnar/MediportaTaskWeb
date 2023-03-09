using MediportaTaskWeb.Data.Http;
using MediportaTaskWeb.Dto;

namespace MediportaTaskWeb.Dao.TagDAO
{
    public class TagDAO : ITagDAO
    {
        private readonly IHttpClientService _httpClientService;
        public TagDAO(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<List<TagDTO>?> GetTagsByUsageDescAsync(int page, int pageSize)
        {
            string uri = $"https://api.stackexchange.com/2.3/tags?order=desc&sort=popular&site=stackoverflow&page={page}&pagesize={pageSize}";
            TagPageDTO? tagPageDTO = await _httpClientService.GetAsync<TagPageDTO>(uri, true);
            return tagPageDTO != null ? tagPageDTO.Items : null;
        }
    }
}
