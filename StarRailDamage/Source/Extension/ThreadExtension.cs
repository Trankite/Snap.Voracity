namespace StarRailDamage.Source.Extension
{
    public static class ThreadExtension
    {
        public static Thread SetBackground(this Thread thread)
        {
            return thread.Configure(thread.IsBackground = true);
        }

        public static void STAStart(this Thread thread)
        {
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}