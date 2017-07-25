using System;
using System.Collections.Generic;

using CatFight.Data;
using CatFight.Players.Schematics;
using CatFight.Util;

namespace CatFight.Fighters.Loadouts
{
    [Serializable]
    public sealed class SpecialLoadoutSlot : LoadoutSlot
    {
        private readonly Dictionary<string, int> _specialTypeVotes = new Dictionary<string, int>();

        public IReadOnlyDictionary<string, int> Specials => _specialTypeVotes;

        public SpecialLoadoutSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }

        public override void Process(SchematicSlot schematicSlot)
        {
            SpecialSchematicSlot specialSlot = (SpecialSchematicSlot)schematicSlot;
            if(null == specialSlot.SpecialItem) {
                return;
            }

            int currentCount = _specialTypeVotes.GetOrDefault(specialSlot.SpecialItem.Type);
            _specialTypeVotes[specialSlot.SpecialItem.Type] = currentCount + 1;
        }

        public override void Complete()
        {
        }
    }
}
