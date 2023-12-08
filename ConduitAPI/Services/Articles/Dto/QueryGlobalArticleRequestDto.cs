using ConduitAPI.Infrastructure.CommonDto;

namespace ConduitAPI.Services.Articles.Dto
{
    public class QueryGlobalArticleRequestDto : PagingRequestDto
    {
        public string Tag { get; init; }
        public string Author { get; init; }
        public string Favorited { get; init; }
    }
}
