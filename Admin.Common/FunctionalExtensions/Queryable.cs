using System;
using System.Linq;
using System.Linq.Expressions;

namespace Admin.Common.FunctionalExtensions
{
    public static class Queryable
    {
        public static IQueryable<T> FilterRepositoryBy<T>(this IQueryable<T> @this,
            Func<bool> predicate,
            Expression<Func<T, bool>> filterPredicate) => predicate()
            ? @this.Where(filterPredicate)
            : @this;

    }
}
