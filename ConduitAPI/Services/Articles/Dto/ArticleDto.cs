using ConduitAPI.Services.Profile.Dto;

namespace ConduitAPI.Services.Articles.Dto
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Slug { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
		public string Content { get; init; }
		public bool Favorited { get; init; }
		public string[] TagList { get; init; }
		public int FavoritesCount { get; init; }
		public ProfileDto Author { get; init; }

    }
}
