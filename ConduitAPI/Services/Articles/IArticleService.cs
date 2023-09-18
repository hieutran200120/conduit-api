using ConduitAPI.Infrastructure.CommonDto;
using ConduitAPI.Services.Articles.Dto;

namespace ConduitAPI.Services.Articles
{
    public interface IArticleService
    {
        public Task<PagingResponseDto<ArticleDto>> GetGlobalArticle(QueryGlobalArticleRequestDto request);
    }
}
