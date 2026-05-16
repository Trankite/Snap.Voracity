using SourceBuilder.Source;
using SourceBuilder.Source.Binding;

namespace SourceBuilder
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            using CodeBuilder Builder = new();
            PropertyBinding.BuildCode(Builder, "name", "propertyType", "ownerType", "defaultValue", true);
        }
    }
}