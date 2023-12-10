using ConduitAPI.Infrastructure.CommonDto;
using ConduitAPI.Infrastructure.LinQ;
using ConduitAPI.Services.Comments.Dto;

namespace ConduitAPI.Services.Comments
{
	public interface ICommentServicecs
	{
		public Task<PagingResponseDto<CommentDto>> GetComment(CommentDto request);
		public Task<CommentDto> PostComment(CommentDto request, string slug);
		public Task<CommentDto> DeleteComment(int id, string slug);
	}
}
