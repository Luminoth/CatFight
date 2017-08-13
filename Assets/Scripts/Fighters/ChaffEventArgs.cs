using System;

using CatFight.Items.Specials;

namespace CatFight.Fighters
{
    public sealed class ChaffEventArgs : EventArgs
    {
        public Chaff Chaff { get; set; }
    }
}
