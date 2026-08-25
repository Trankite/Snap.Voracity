using System.Text;

namespace Common.Source.Extension
{
    public static class StringBuilderExtension
    {
        public static string Complete(this StringBuilder builder)
        {
            return builder.ToString().Configure(builder.Clear());
        }
    }
}