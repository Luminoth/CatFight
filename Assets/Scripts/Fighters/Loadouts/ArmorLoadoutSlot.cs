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
            ArmorData.ArmorDataEntry armorItem = armorSlot.ArmorItem;
            if(null == armorItem) {
                return;
            }

            int currentCount = _armorTypeVotes.GetOrDefault(armorItem.Id);
            _armorTypeVotes[armorItem.Id] = currentCount + 1;
        }

        public override void Complete()
        {
        }
    }
}
