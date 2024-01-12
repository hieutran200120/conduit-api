using ConduitAPI.Infrastructure.CommonDto;
using ConduitAPI.Services.Profile.Dto;

namespace ConduitAPI.Services.Comments.Dto
{
	public class CommentDto
	{
		public string Slug { get; init; }
		public string CommentContent { get; init; }
		public ProfileDto Author { get; init; }
	}
}
