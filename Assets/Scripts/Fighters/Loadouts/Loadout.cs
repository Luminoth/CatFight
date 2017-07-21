using System.Collections.Generic;

using CatFight.Players;
using CatFight.Players.Schematics;

namespace CatFight.Fighters
{
    public sealed class Loadout
    {
        private readonly Dictionary<int, LoadoutSlot> _slots = new Dictionary<int, LoadoutSlot>();

        public Loadout(IReadOnlyCollection<Player> team)
        {
            foreach(Player player in team) {
                Schematic schematic = player.Schematic;
            }
        }
    }
}
