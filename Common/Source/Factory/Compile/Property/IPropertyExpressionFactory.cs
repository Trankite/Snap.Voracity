using System.Linq.Expressions;

namespace Common.Source.Factory.Compile.Property
{
    public interface IPropertyExpressionFactory<TSender, TProperty>
    {
        IPropertyExpression<TSender, TProperty> GetPropertyExpression(Expression<Func<TSender, TProperty>> expression);
    }
}