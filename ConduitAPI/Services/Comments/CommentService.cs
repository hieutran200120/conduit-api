﻿using ConduitAPI.Entities;
using ConduitAPI.Infrastructure.Auth;
using ConduitAPI.Infrastructure.CommonDto;
using ConduitAPI.Infrastructure.LinQ;
using ConduitAPI.Services.Articles.Dto;
using ConduitAPI.Services.Comments.Dto;
using ConduitAPI.Services.Profile.Dto;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace ConduitAPI.Services.Comments
{
	public class CommentService:ICommentServicecs
	{
		private readonly MainDbContext _mainDbContext;
		private readonly ICurrentUser _currentUser;
		public CommentService(MainDbContext mainDbContext, ICurrentUser currentUser)
		{
			_mainDbContext = mainDbContext;
			_currentUser = currentUser;
		}
		public async Task<PagingResponseDto<CommentDto>> GetComment(CommentDto request)
		{
			var query = await _mainDbContext.Comments
				.Select(x => new CommentDto
				{
					CommentContent = x.CommentContent,
					Author = new ProfileDto
					{
						Bio = x.User.Bio,
						Image = x.User.Image,
						Username = x.User.Username,
						Following = x.User.Followers.Any(x => x.FollowerId == _currentUser.Id),
					},
					
				})
				 .Paging(request.PageIndex, request.Limit).ToListAsync();
			var TotalCount =  query.Count();
			return new PagingResponseDto<CommentDto>
			{
				Items = query,
				TotalCount = TotalCount
			};
		}
		public async Task<CommentDto> PostComment(CommentDto request,string slug)
		{
			var article = await _mainDbContext.Articles
			   .Include(x => x.Comments)
			   .FirstOrDefaultAsync(x => x.Slug == slug);
			var author = await _mainDbContext.Users.FirstAsync(x => x.Id == _currentUser.Id);
			var comment = new Comment
			{
				User = author,
				CommentContent = request.CommentContent,
			};
			_mainDbContext.Comments.Add(comment);
			article.Comments.Add(comment);
			await _mainDbContext.SaveChangesAsync();
			return new CommentDto
			{
				/*Author = author,*/
				CommentContent = comment.CommentContent
			};
		}
		public async Task<CommentDto>DeleteComment(int id, string slug)
		{
			var article = await _mainDbContext.Articles
			   .Include(x => x.Comments)
			   .FirstOrDefaultAsync(x => x.Slug == slug);
			var comment = await _mainDbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
			_mainDbContext.Comments.Remove(comment);
			await _mainDbContext.SaveChangesAsync();
			return new CommentDto
			{
				/*Author = author,*/
				CommentContent = comment.CommentContent
			};
		}
	}
}