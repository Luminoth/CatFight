using System;

using CatFight.Data;

namespace CatFight.Items.Specials
{
    [Serializable]
    public sealed class Chaff : Special
    {
        public override SpecialData.SpecialType SpecialType => SpecialData.SpecialType.Chaff;

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
