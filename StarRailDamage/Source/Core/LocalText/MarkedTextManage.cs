using StarRailDamage.Source.Model.Text;
using StarRailDamage.Source.Resource.Localization;
using System.Globalization;
using System.Resources;

namespace StarRailDamage.Source.Core.LocalText
{
    public class MarkedTextManage : LocalTextManage
    {
        public static readonly LocalTextManage TextManage = new MarkedTextManage();

        protected override ResourceManager Manager => MarkedText.ResourceManager;

        public override CultureInfo Culture
        {
            get => MarkedText.Culture;
            set => MarkedText.Culture = OnUICultureChanged(value);
        }

        public static TextBinding Binding(string target) => TextManage.GetBinding(target);
    }
}