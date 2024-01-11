
using Azure.Core;
using ConduitAPI.Entities;
using ConduitAPI.Infrastructure.Auth;
using ConduitAPI.Infrastructure.CommonDto;
using ConduitAPI.Infrastructure.Exceptions;
using ConduitAPI.Infrastructure.LinQ;
using ConduitAPI.Services.Articles.Dto;
using ConduitAPI.Services.Profile.Dto;
using ConduitAPI.Services.Users.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;



namespace ConduitAPI.Services.Articles
{

    public class ArticleService : IArticleService
    {
        private readonly MainDbContext _mainDbContext;
		private readonly IAuthService _authService;
		private readonly ICurrentUser _currentUser;
		private readonly ILogger<ArticleService> _logger;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public ArticleService(MainDbContext mainDbContext, IAuthService authService, ICurrentUser currentUser, ILogger<ArticleService> logger, IHttpContextAccessor httpContextAccessor)
		{
			_mainDbContext = mainDbContext;
			_authService = authService;
			_currentUser = currentUser;
			_logger = logger;
			_httpContextAccessor = httpContextAccessor;
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
					Id = x.Id,
                    Description = x.Description,
                    Slug = x.Slug,
                    Title = x.Title,
                    Content = x.Content,
                    Author = new ProfileDto
                    {
                        Bio = x.User.Bio,
                        Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{x.User.Image}",
						Username = x.User.Username,
						Following = x.User.Followers.Any(x => x.FollowerId == _currentUser.Id),
					},
					FavoritesCount = x.Favorites.Count()
				})
                 /*.Paging(request.PageIndex, request.Limit)*/.ToListAsync();
            var TotalCount = await query.CountAsync();
            return new PagingResponseDto<ArticleDto>
            {
                Items = items,
				TotalCount=TotalCount
			};
        }
		public async Task<ArticleDto> GetArticle(string slug)
		{
			var query = await _mainDbContext.Articles
				.Where(x => x.Slug == slug)
				.Select(x => new ArticleDto
				{
					Id = x.Id,
					Description = x.Description,
					Slug = x.Slug,
					Title = x.Title,
					Content = x.Content,
					Author = new ProfileDto
					{
						Bio = x.User.Bio,
						Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{x.User.Image}",
						Username = x.User.Username,
						Following = x.User.Followers.Any(x => x.FollowerId == _currentUser.Id),
					},
					/*Favorited=x.Favorites.Any(x=>x.ArticleId==_currentUser.Id)*/

					FavoritesCount = x.Favorites.Count()
				})
				.FirstOrDefaultAsync();

			return query;
		}
		public async Task<PostArticleDto> PostArticle(PostArticleDto request)
		{


			var article = new Article
			{
				Slug = request.Title.GenerateSlug(),
				Title = request.Title,
				Description = request.Description,
				Content = request.Content,
				UserId = (Guid)_currentUser.Id,
				Tags = request.TagList
			};
			await _mainDbContext.Articles.AddAsync(article);
			await _mainDbContext.SaveChangesAsync();

			return new PostArticleDto
			{
				Id = article.Id,
				Slug = article.Title.GenerateSlug(),
				Title = article.Title,
				Description = article.Description,
				Content = article.Content,
				TagList = article.Tags,
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
