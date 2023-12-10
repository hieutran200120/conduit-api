using ConduitAPI.Services.Articles;
using ConduitAPI.Services.Articles.Dto;
using ConduitAPI.Services.Comments;
using ConduitAPI.Services.Comments.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConduitAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentController : ControllerBase
	{
		private readonly ICommentServicecs _commentServicecs;
		public CommentController(ICommentServicecs commentServicec)
		{
			_commentServicecs = commentServicec;
		}
		[HttpGet]
		public async Task<IActionResult> GetComment(CommentDto request)
		{
			var response = await _commentServicecs.GetComment(request);
			return Ok(response);
		}
		[HttpPost("{slug}/comments")]
		public async Task<IActionResult> PostComment(CommentDto request, string slug)
		{
			var response = await _commentServicecs.PostComment(request,slug);
			return Ok(response);
		}
		[HttpDelete("{slug}/comments/{id}")]
		public async Task<IActionResult> DeleteComment(int id, string slug)
		{
			var response = await _commentServicecs.DeleteComment(id, slug);
			return Ok(response);
		}
	}
}
