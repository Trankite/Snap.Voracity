using StarRailDamage.Source.Extension;

namespace StarRailDamage.Source.UI.Factory.PropertyBinding
{
    public class PropertyBinding<TSender> : IPropertyBinding<TSender>
    {
        public Action<TSender> ModelHanlder { get; }

        public Action<TSender> DependHanlder { get; }

        public PropertyBindingMode BindingMode { get; }

        public PropertyBinding(PropertyBindingMode bindingMode, Action<TSender> modelHanlder, Action<TSender> dependHanlder)
        {
            ModelHanlder = modelHanlder;
            DependHanlder = dependHanlder;
            BindingMode = bindingMode;
        }

        public bool IsModelToDepend => BindingMode.HasFlag(PropertyBindingMode.OneWay);

        public bool IsDependToModel => BindingMode.HasFlag(PropertyBindingMode.OneWayToSource);

        public bool DependToModel(TSender sender)
        {
            return PropertyBinding<TSender>.PropertyChange(sender, IsDependToModel, DependHanlder);
        }

        public bool ModelToDepend(TSender sender)
        {
            return PropertyBinding<TSender>.PropertyChange(sender, IsModelToDepend, ModelHanlder);
        }

        private static bool PropertyChange(TSender sender, bool hasFlag, Action<TSender> handler)
        {
            return hasFlag && true.Configure(handler, sender);
        }
    }
}