using StarRailDamage.Source.Model.Text;
using System.Globalization;
using System.Resources;

namespace StarRailDamage.Source.Core.LocalText
{
    public class FixedTextManage : LocalTextManage
    {
        private static CultureInfo ResourceCulture;

        private static readonly ResourceManager ResourceManager;

        public static readonly LocalTextManage TextManage = new FixedTextManage();

        protected override ResourceManager Manager => ResourceManager;

        public override CultureInfo Culture
        {
            get => ResourceCulture;
            set => ResourceCulture = OnUICultureChanged(value);
        }

        public static TextBinding Binding(string target) => TextManage.GetBinding(target);

        static FixedTextManage()
        {
            ResourceCulture = CultureInfo.CurrentCulture;
            ResourceManager = new ResourceManager("StarRailDamage.Source.Resource.Localization.FixedText", typeof(Program).Assembly);
        }
    }
}