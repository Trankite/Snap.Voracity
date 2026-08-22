namespace Common.Source.Model.StarRail.Character.Damage
{
    public interface ICharacterDamageModel
    {
        double Normal { get; set; }

        double Critical { get; set; }

        double Elation { get; set; }

        double Break { get; set; }

        double SuperBreak { get; set; }

        double Delay { get; set; }
    }
}