namespace StarRailDamage.Source.Extension
{
    public static class IntExtension
    {
        public static int Parse(ReadOnlySpan<char> value)
        {
            return int.TryParse(value, out int Result) ? Result : 0;
        }
    }
}