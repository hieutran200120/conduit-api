using ConduitAPI.Infrastructure.CommonDto;
using ConduitAPI.Services.Articles.Dto;

namespace ConduitAPI.Services.Articles
{
    public interface IArticleService
    {
         Task<PagingResponseDto<ArticleDto>> GetGlobalArticle(QueryGlobalArticleRequestDto request);
		Task<PostArticleDto> PostArticle(PostArticleDto request);

		 Task<UpdateArticleDto> UpdateArticle(string slug, UpdateArticleDto request);
		 Task<ArticleDto> DeleteArticle(string slug);
		 Task<ArticleDto> GetArticle(string slug);
	}
}
