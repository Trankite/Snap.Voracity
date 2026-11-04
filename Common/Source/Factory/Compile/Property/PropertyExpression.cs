using Common.Source.Extension;

namespace Common.Source.Factory.Compile.Property
{
    public class PropertyExpression<TSender, TValue> : IPropertyExpression<TSender, TValue>
    {
        public TSender? Sender { get; set; }

        public Func<TSender, TValue> GetValue { get; }

        public Action<TSender, TValue?> SetValue { get; }

        public Func<TSender?, bool> NullCheck { get; } = ObjectExtension.IsNotNull;

        public PropertyExpression(Func<TSender, TValue> getValue, Action<TSender, TValue?> setValue)
        {
            GetValue = getValue;
            SetValue = setValue;
        }

        public PropertyExpression(Func<TSender, TValue> getter, Action<TSender, TValue?> setter, TSender? sender) : this(getter, setter)
        {
            Sender = sender;
        }

        public PropertyExpression(Func<TSender, TValue> getter, Action<TSender, TValue?> setter, Func<TSender?, bool> nullCheck) : this(getter, setter)
        {
            NullCheck = nullCheck;
        }
    }
}