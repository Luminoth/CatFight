using System;

using CatFight.Data;

namespace CatFight.Items.Specials
{
    [Serializable]
    public sealed class Missiles : Special
    {
        public Missiles(SpecialData.SpecialDataEntry specialData, int totalUses)
            : base(specialData, totalUses)
        {
        }

        protected override void DoUse()
        {
UnityEngine.Debug.LogError("TODO: use missiles");
        }
    }
}
