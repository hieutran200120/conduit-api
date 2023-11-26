using ConduitAPI.Entities;
using ConduitAPI.Infrastructure.Auth;
using ConduitAPI.Infrastructure.CommonDto;
using ConduitAPI.Infrastructure.Exceptions;
using ConduitAPI.Infrastructure.LinQ;
using ConduitAPI.Services.Articles.Dto;
using ConduitAPI.Services.Profile.Dto;
using Microsoft.EntityFrameworkCore;

namespace ConduitAPI.Services.Articles
{

    public class ArticleService : IArticleService
    {
        private readonly MainDbContext _mainDbContext;
		private readonly IAuthService _authService;
		private readonly ICurrentUser _currentUser;
		public ArticleService(MainDbContext mainDbContext, IAuthService authService, ICurrentUser currentUser)
        {
            _mainDbContext = mainDbContext;
            _authService = authService;
            _currentUser = currentUser;
        }
        public async Task<PagingResponseDto<ArticleDto>> GetGlobalArticle(QueryGlobalArticleRequestDto request)
        {
            var query = _mainDbContext.Articles
                .WhereIf(!string.IsNullOrEmpty(request.Author), x => x.User.Username == request.Author)
                .WhereIf(!string.IsNullOrEmpty(request.Favorited), x => x.Favorites.Any(i => i.User.Username == request.Favorited))
                .WhereIf(!string.IsNullOrEmpty(request.Tag), x => x.Tags.Contains(request.Tag))
                .OrderByDescending(x => x.CreatedAt);

            var items = await query
                .Select(x => new ArticleDto
                {
                    Description = x.Description,
                    Slug = x.Slug,
                    Title = x.Title,
                    Content = x.Content,
                    Author = new ProfileDto
                    {
                        Bio = x.User.Bio,
                        Image = x.User.Image,
                        Username = x.User.Username,
                        Following = false
                    }
                })
                 .Paging(request.PageIndex, request.Limit).ToListAsync();
            var TotalCount = await query.CountAsync();
            return new PagingResponseDto<ArticleDto>
            {
                Items = items

            };
        }
        public async Task<PostArticleDto>PostArticle(PostArticleDto request)
        {
            var author = await _mainDbContext.Users.FirstAsync(x => x.Id == _currentUser.Id);
            var article = new Article
            {
                Slug= request.Title.GenerateSlug(),
                Title = request.Title,
                Description = request.Description,
                Content=request.Content,
				UserId =author.Id,
            };
            await _mainDbContext.Articles.AddAsync(article);
            await _mainDbContext.SaveChangesAsync();
            return new PostArticleDto
            {
                Slug = article.Title.GenerateSlug(),
                Title = article.Title,
                Description = article.Description,
                Content=article.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,              
            };
        }
        public async Task<UpdateArticleDto> UpdateArticle( string Slug, UpdateArticleDto request)
        {
            var article= await _mainDbContext.Articles.Where(x=>x.Slug == Slug).FirstOrDefaultAsync();
            if(article == null)
            {
				throw new RestException(System.Net.HttpStatusCode.NotFound, "No article");
			}    
            article.Title = request.Title;
            article.Description = request.Description;
            article.Content =request.Content;
            article.Slug= request.Title.GenerateSlug();
            await _mainDbContext.SaveChangesAsync();
            return new UpdateArticleDto
			{
                Title = article.Title,
                Description = article.Description,
                Content=article.Content,
            }; 
        }
        public async Task<ArticleDto> DeleteArticle(string Slug)
        {
            var article= _mainDbContext.Articles.Where(x=>x.Slug==Slug).FirstOrDefault();
             _mainDbContext.Articles.Remove(article);
            await _mainDbContext.SaveChangesAsync();
            return new ArticleDto
            {
				Description = article.Description,
				Slug = article.Title.GenerateSlug(),
				Title = article.Title,
				Content = article.Content				
			};

        }    

    }
}
