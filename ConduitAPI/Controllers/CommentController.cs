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
		public async Task<IActionResult> GetComment()
		{
			var response = await _commentServicecs.GetComment();
			return Ok(response);
		}
		[HttpPost("{slug}")]
		public async Task<IActionResult> PostComment([FromBody] PostCommentDto request)
		{
			var response = await _commentServicecs.PostComment(request);
			return Ok(response);
		}
		[HttpDelete("{slug}/{id}")]
		public async Task<IActionResult> DeleteComment(int id, string slug)
		{
			var response = await _commentServicecs.DeleteComment(id, slug);
			return Ok(response);
		}
	}
}
