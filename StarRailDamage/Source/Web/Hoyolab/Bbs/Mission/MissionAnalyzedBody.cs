namespace StarRailDamage.Source.Web.Hoyolab.Bbs.Mission
{
    public class MissionAnalyzedBody
    {
        public int TotalPoint { get; set; }

        public int TodayPoint { get; set; }

        public int Surplus { get; set; }

        public Dictionary<MissionType, int> Mission { get; set; } = [];

        public MissionAnalyzedBody() { }

        public MissionAnalyzedBody(int totalPoint, int todayPoint, int surplus)
        {
            TotalPoint = totalPoint;
            TodayPoint = todayPoint;
            Surplus = surplus;
        }
    }
}