using System;

using CatFight.Data;
using CatFight.Fighters;

namespace CatFight.Items.Specials
{
    [Serializable]
    public sealed class Chaffs : Special
    {
        public Chaffs(Fighter fighter, SpecialData.SpecialDataEntry specialData, int totalUses)
            : base(fighter, specialData, totalUses)
        {
        }

        protected override void DoUse()
        {
            // spawn N chaff

UnityEngine.Debug.LogError("TODO: use chaff");
        }
    }
}
