using System;
using System.Collections.Generic;

using CatFight.Data;
using CatFight.Players.Schematics;
using CatFight.Util;

namespace CatFight.Fighters.Loadouts
{
    [Serializable]
    public sealed class ArmorLoadoutSlot : LoadoutSlot
    {
        private readonly Dictionary<string, int> _armorTypeVotes = new Dictionary<string, int>();

        public ArmorLoadoutSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }

        public override void Process(SchematicSlot schematicSlot)
        {
            ArmorSchematicSlot armorSlot = (ArmorSchematicSlot)schematicSlot;
            if(null == armorSlot.ArmorItem) {
                return;
            }

            int currentCount = _armorTypeVotes.GetOrDefault(armorSlot.ArmorItem.Type);
            _armorTypeVotes[armorSlot.ArmorItem.Type] = currentCount + 1;
        }

        public override void Complete()
        {
// TODO: armor is a combination of anti-stuff at vote-strength
        }
    }
}
