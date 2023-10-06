using ConduitAPI.Infrastructure.CommonDto;
using ConduitAPI.Infrastructure.LinQ;
using ConduitAPI.Services.Articles.Dto;
using ConduitAPI.Services.Profile.Dto;
using Microsoft.EntityFrameworkCore;

namespace ConduitAPI.Services.Articles
{

    public class ArticleService : IArticleService
    {
        private readonly MainDbContext _mainDbContext;
        public ArticleService(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }
        public async Task<PagingResponseDto<ArticleDto>> GetGlobalArticle(QueryGlobalArticleRequestDto request)
        {
            var query = _mainDbContext.Articles
                .WhereIf(!string.IsNullOrEmpty(request.Author), x => x.User.Username == request.Author)
                .WhereIf(!string.IsNullOrEmpty(request.Favorited), x => x.Favorites.Any(i => i.User.Username == request.Favorited))
                .WhereIf(!string.IsNullOrEmpty(request.Tag), x => x.Tags.Contains(request.Tag))
                .OrderByDescending(x => x.CreatedAt);
            var offset = Math.Max(request.Offset, 1);
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.Limit);
            var items = await query
                .Select(x => new ArticleDto
                {
                    Description = x.Description,
                    Slug = x.Slug,
                    Title = x.Title,
                    Author = new ProfileDto
                    {
                        Bio = x.User.Bio,
                        Image = x.User.Image,
                        Username = x.User.Username,
                        Following = false //TODO: iplement later when implement authenticate
                    }
                })
                 .Skip((offset - 1) * request.Limit).Take(request.Limit).ToListAsync();
            return new PagingResponseDto<ArticleDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
