using System;

namespace CatFight.Items.Specials
{
    [Serializable]
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
