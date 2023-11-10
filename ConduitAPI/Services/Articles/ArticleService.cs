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
                    Conten=x.Conten,
                    Author = new ProfileDto
                    {
                        Bio = x.User.Bio,
                        Image = x.User.Image,
                        Username = x.User.Username,
                        Following = false 
                    }
                })
                 .Paging(request.PageIndex,request.Limit).ToListAsync();
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
                Slug=request.Slug,
                Title = request.Title,
                Description = request.Description,
                Conten=request.Conten,
				UserId =author.Id,
            };
            await _mainDbContext.Articles.AddAsync(article);
            await _mainDbContext.SaveChangesAsync();
            return new PostArticleDto
            {
                Slug = article.Title.GenerateSlug(),
                Title = article.Title,
                Description = article.Description,
                Conten=article.Conten,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,              
            };
        }
        public async Task<UpdateArticleDto> UpdateArticle( string Slug, UpdateArticleDto request)
        {
            var article= _mainDbContext.Articles.Where(x=>x.Slug == Slug).FirstOrDefault();
            if(article == null)
            {
				throw new RestException(System.Net.HttpStatusCode.NotFound, "No article");
			}    
            article.Title = request.Title;
            article.Description = request.Description;
            article.Conten =request.Conten;
            article.Slug= request.Title.GenerateSlug();
            await _mainDbContext.SaveChangesAsync();
            return new UpdateArticleDto
			{
                Title = article.Title,
                Description = article.Description,
                Conten=article.Conten,
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
				Slug = article.Slug,
				Title = article.Title,
				Conten = article.Conten				
			};

        }    

    }
}
