using System;

using CatFight.Data;

namespace CatFight.Items.Specials
{
    [Serializable]
    public sealed class Chaffs : Special
    {
        public Chaffs(SpecialData.SpecialDataEntry specialData, int totalUses)
            : base(specialData, totalUses)
        {
        }

        protected override void DoUse()
        {
            // spawn N chaff

UnityEngine.Debug.LogError("TODO: use chaff");
        }
    }
}
