using Common.Source.Extension;
using System.Linq.Expressions;

namespace Common.Source.Factory.Compile.Property
{
    public class PropertyExpressionFactory<TSender, TProperty> : IPropertyExpressionFactory<TSender, TProperty>
    {
        public IPropertyExpression<TSender, TProperty> GetPropertyExpression(Expression<Func<TSender, TProperty>> expression)
        {
            return new PropertyExpression<TSender, TProperty>(expression.Compile(), GetPropertySetter(expression).Compile(), GetNullCheck(expression).Compile());
        }

        public static Expression<Func<TSender?, bool>> GetNullCheck(Expression<Func<TSender, TProperty>> expression)
        {
            ParameterExpression parameter = expression.Parameters.First();
            Expression nullCheckExpression = BuildNullCheck(expression.Body.MemberExpression().ThrowIfNull());
            return Expression.Lambda<Func<TSender?, bool>>(nullCheckExpression, parameter);
        }

        public static Expression<Action<TSender, TProperty?>> GetPropertySetter(Expression<Func<TSender, TProperty>> expression)
        {
            ParameterExpression senderParameter = Expression.Parameter(typeof(TSender));
            ParameterExpression propertyParameter = Expression.Parameter(typeof(TProperty));
            MemberExpression memberExpression = expression.Body.MemberExpression().ThrowIfNull();
            MemberExpression propertyAccess = BuildPropertyAccess(senderParameter, memberExpression);
            BinaryExpression assignExpression = Expression.Assign(propertyAccess, propertyParameter);
            return Expression.Lambda<Action<TSender, TProperty?>>(assignExpression, senderParameter, propertyParameter);
        }

        private static Expression BuildNullCheck(MemberExpression memberExpression)
        {
            if (memberExpression.Expression is MemberExpression parentMemberExpression)
            {
                return Expression.OrElse(BuildNullCheck(parentMemberExpression), Expression.Equal(parentMemberExpression, Expression.Constant(null)));
            }
            else if (memberExpression.Expression is not ParameterExpression)
            {
                return Expression.Equal(memberExpression, Expression.Constant(null));
            }
            return Expression.Constant(false);
        }

        private static MemberExpression BuildPropertyAccess(ParameterExpression parameterExpression, MemberExpression memberExpression)
        {
            if (memberExpression.Expression is MemberExpression parentMemberExpression)
            {
                return Expression.MakeMemberAccess(BuildPropertyAccess(parameterExpression, parentMemberExpression), memberExpression.Member);
            }
            return Expression.MakeMemberAccess(parameterExpression, memberExpression.Member);
        }
    }
}