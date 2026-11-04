namespace Common.Source.Web.Hoyolab.Takumi.Note
{
    public class NoteStamina
    {
        public int Current { get; set; }

        public int Maximum { get; set; }

        public DateTimeOffset FullTime { get; set; }

        public NoteStamina() { }

        public NoteStamina(int current, int maximum)
        {
            Current = current;
            Maximum = maximum;
        }

        public NoteStamina(int current, int maximum, DateTimeOffset fullTime) : this(current, maximum)
        {
            FullTime = fullTime;
        }
    }
}