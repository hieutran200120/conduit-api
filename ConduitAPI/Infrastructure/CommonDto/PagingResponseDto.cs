namespace ConduitAPI.Infrastructure.CommonDto
{
    public class PagingResponseDto<T>
    {
        public List<T> Items { get; init; }
        public int TotalCount { get; init; }
    }
}
