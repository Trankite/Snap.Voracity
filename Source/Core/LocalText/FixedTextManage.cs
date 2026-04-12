using StarRailDamage.Source.Model.Text;
using StarRailDamage.Source.Resource.Localization;
using System.Globalization;
using System.Resources;

namespace StarRailDamage.Source.Core.LocalText
{
    public class FixedTextManage : LocalTextManage
    {
        public static readonly LocalTextManage TextManage = new FixedTextManage();

        protected override ResourceManager Manager => FixedText.ResourceManager;

        public override CultureInfo Culture
        {
            get => FixedText.Culture;
            set => FixedText.Culture = OnUICultureChanged(value);
        }

        public new static TextBinding Binding(string target) => TextManage.Binding(target);

        public new static string GetString(string target) => TextManage.GetString(target);
    }
}