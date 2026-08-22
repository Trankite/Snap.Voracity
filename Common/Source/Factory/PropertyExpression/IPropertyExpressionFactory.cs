using System.Linq.Expressions;

namespace Common.Source.Factory.PropertyExpression
{
    public interface IPropertyExpressionFactory<TSender, TProperty>
    {
        IPropertyExpression<TSender, TProperty> GetPropertyExpression(Expression<Func<TSender, TProperty>> expression);
    }
}