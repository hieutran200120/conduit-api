using System.Linq.Expressions;

namespace ConduitAPI.Infrastructure.LinQ
{
    public static class Queryable
    {
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if(!condition)
            {
                return source;
            }
            else
            {
                return source.Where(predicate);
            }
        }
        public static IQueryable<TSource> Paging<TSource>(this IQueryable<TSource> source, int pageSize, int pageIndex)
        {
            var offset = (Math.Max(pageIndex, 1) - 1) * pageSize;
            return source.Skip(offset).Take(pageSize);
        }
    }
}
