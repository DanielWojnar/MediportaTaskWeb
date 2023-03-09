using AutoMapper;
using MediportaTaskWeb.Dao.TagDAO;
using MediportaTaskWeb.Dto;
using MediportaTaskWeb.Models;

namespace MediportaTaskWeb.Services.TagService
{
    public class TagService : ITagService
    {
        private readonly ITagDAO _tagDao;
        private readonly IMapper _mapper;
        private readonly ILogger<TagService> _logger;
        public TagService(ITagDAO tagDao, IMapper mapper, ILogger<TagService> logger) { 
            _tagDao = tagDao;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<Tag>> GetMostPopularTagsAsync(int amount)
        {
            try {
                List<TagDTO> tagsDto = new List<TagDTO>();
                int page = 1;
                int retries = 1;
                int pageSize = 0;
                while (amount > 0)
                {
                    if(amount > Constants.Tag.MaxPageSize)
                    {
                        pageSize = Constants.Tag.MaxPageSize;
                    }
                    else
                    {
                        pageSize = amount;
                    }

                    List<TagDTO>? newTagsDto = await _tagDao.GetTagsByUsageDescAsync(page, pageSize);
                    if (newTagsDto == null)
                    {
                        if (retries > Constants.Tag.MaxRetries)
                        {
                            throw new HttpRequestException($"{nameof(_tagDao)} : {nameof(_tagDao.GetTagsByUsageDescAsync)} returned null in every of {Constants.Tag.MaxRetries + 1} tries.");
                        }
                        retries++;
                        continue;
                    }

                    tagsDto.AddRange(newTagsDto);
                    amount = amount - pageSize;
                    pageSize = 0;
                    retries = 0;
                    page++;
                }

                List<Tag> tags = _mapper.Map<List<Tag>>(tagsDto);
                var tagsSum = tags.Sum(x => x.Count);
                for(int i = 0; i < tags.Count; i++)
                {
                    tags[i].ProcentageInScope = ((double)(tags[i].Count * 100)) / ((double)tagsSum);
                    tags[i].Index = i + 1;
                }

                return tags;
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return new List<Tag>();
            }
        }
    }
}
