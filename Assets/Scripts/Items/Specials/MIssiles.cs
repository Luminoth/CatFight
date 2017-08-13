using System;

using CatFight.Data;
using CatFight.Fighters;

namespace CatFight.Items.Specials
{
    [Serializable]
    public sealed class Missiles : Special
    {
        public Missiles(Fighter fighter, SpecialData.SpecialDataEntry specialData, int totalUses)
            : base(fighter, specialData, totalUses)
        {
        }

        protected override void DoUse()
        {
            // spawn N missiles
            // spawn target
            // missiles should track target as they fly

UnityEngine.Debug.LogError("TODO: use missiles");
        }
    }
}
