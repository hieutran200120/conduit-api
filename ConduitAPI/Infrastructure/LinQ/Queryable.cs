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
    }
}
