namespace Common.Source.Factory.Compile.Property
{
    public interface IPropertyExpression<TSender, TValue>
    {
        Func<TSender, TValue> GetValue { get; }

        Action<TSender, TValue?> SetValue { get; }

        Func<TSender?, bool> NullCheck { get; }
    }
}