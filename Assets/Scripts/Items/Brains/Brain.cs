namespace CatFight.Items.Brains
{
    public abstract class Armor : Item
    {
        public const string BrainTypeAggressive = "aggressive";
        public const string BrainTypeDefensive = "defensive";

        public abstract string BrainType { get; }
    }
}
