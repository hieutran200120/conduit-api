namespace ConduitAPI.Infrastructure.CommonDto
{
    public class PagingRequestDto
    {
        public int Limit { get; init; } = 20;
        public int Offset { get; init; } = 0;
    }
}
