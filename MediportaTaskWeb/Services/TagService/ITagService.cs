using MediportaTaskWeb.Models;

namespace MediportaTaskWeb.Services.TagService
{
    public interface ITagService
    {
        public Task<List<Tag>> GetMostPopularTagsAsync(int amount);
    }
}
