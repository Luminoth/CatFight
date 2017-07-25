namespace CatFight.Items.Specials
{
    public sealed class Chaff : Special
    {
        public override string SpecialType => SpecialTypeChaff;

        public Chaff(int totalUses)
            : base(totalUses)
        {
        }

        protected override void DoUse()
        {
// TODO
        }
    }
}
