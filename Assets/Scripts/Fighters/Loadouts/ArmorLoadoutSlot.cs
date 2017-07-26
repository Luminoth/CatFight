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
        private readonly Dictionary<int, int> _armorTypeVotes = new Dictionary<int, int>();

        public IReadOnlyDictionary<int, int> ArmorTypeVotes => _armorTypeVotes;

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

            int currentCount = _armorTypeVotes.GetOrDefault(armorSlot.ArmorItem.Id);
            _armorTypeVotes[armorSlot.ArmorItem.Id] = currentCount + 1;
        }

        public override void Complete()
        {
        }
    }
}
