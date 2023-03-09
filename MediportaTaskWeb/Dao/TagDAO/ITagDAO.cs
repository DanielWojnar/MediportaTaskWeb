using MediportaTaskWeb.Dto;

namespace MediportaTaskWeb.Dao.TagDAO
{
    public interface ITagDAO
    {
        public Task<List<TagDTO>?> GetTagsByUsageDescAsync(int page, int pageSize);
    }
}
