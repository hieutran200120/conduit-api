using ConduitAPI.Infrastructure.CommonDto;
using ConduitAPI.Services.Profile.Dto;

namespace ConduitAPI.Services.Comments.Dto
{
	public class CommentDto: PagingRequestDto
	{
		public string Slug { get; init; }
		public string CommentContent { get; init; }
		public ProfileDto Author { get; init; }
	}
}
