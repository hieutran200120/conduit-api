using ConduitAPI.Infrastructure.CommonDto;
using ConduitAPI.Infrastructure.LinQ;
using ConduitAPI.Services.Comments.Dto;

namespace ConduitAPI.Services.Comments
{
	public interface ICommentServicecs
	{
		public Task<PagingResponseDto<CommentDto>> GetComment();
		public Task<CommentDto> PostComment(PostCommentDto request);
		public Task<CommentDto> DeleteComment(int id, string slug);
	}
}
