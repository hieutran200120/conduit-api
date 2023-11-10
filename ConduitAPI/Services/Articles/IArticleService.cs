using ConduitAPI.Infrastructure.CommonDto;
using ConduitAPI.Services.Articles.Dto;

namespace ConduitAPI.Services.Articles
{
    public interface IArticleService
    {
        public Task<PagingResponseDto<ArticleDto>> GetGlobalArticle(QueryGlobalArticleRequestDto request);
        public Task<PostArticleDto> PostArticle(PostArticleDto request);
        public Task<UpdateArticleDto> UpdateArticle(string Slug, UpdateArticleDto request);

		public Task<ArticleDto> DeleteArticle(string Slug);
	}
}
