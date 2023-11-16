using ConduitAPI.Services.Profile.Dto;

namespace ConduitAPI.Services.Articles.Dto
{
	public class PostArticleDto
	{
		public string Title { get; init; }

		public string Description { get; init; }

		public string Content { get; init; }

		/*public string[] TagList { get; init; }*/
		public string Slug { get; init; }
		public DateTime CreatedAt { get; init; }
		public DateTime UpdatedAt { get; init;}
	}
}
