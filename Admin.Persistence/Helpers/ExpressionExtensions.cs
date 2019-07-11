using System;
using System.Linq.Expressions;

namespace PartnerUser.Persistence.Helpers
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>> AndAlso<T>(Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var combined = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    left.Body,
                    new ExpressionParameterReplacer(right.Parameters, left.Parameters).Visit(right.Body) ?? throw new InvalidOperationException("Null Expression")
                ), left.Parameters);

            return combined;
        }

        public static Expression<Func<Entity.PartnerUser, bool>> AppendExpression(Expression<Func<Entity.PartnerUser, bool>> left,
            Expression<Func<Entity.PartnerUser, bool>> right)
        {
            if (left == null)
            {
                left = model => true;
            }

            var result = AndAlso(left, right);
            return result;

        }
    }
}
