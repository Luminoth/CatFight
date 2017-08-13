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
        private readonly Dictionary<int, int> _specialTypeVotes = new Dictionary<int, int>();

        public IReadOnlyDictionary<int, int> SpecialVotes => _specialTypeVotes;

        public SpecialLoadoutSlot(Fighter fighter, SchematicSlotData slotData)
            : base(fighter, slotData)
        {
        }

        public override void Process(SchematicSlot schematicSlot)
        {
            SpecialSchematicSlot specialSlot = (SpecialSchematicSlot)schematicSlot;
            if(null == specialSlot.SpecialItem) {
                return;
            }

            int currentCount = _specialTypeVotes.GetOrDefault(specialSlot.SpecialItem.Id);
            _specialTypeVotes[specialSlot.SpecialItem.Id] = currentCount + 1;
        }

        public override void Complete()
        {
        }

// TODO: there should be a way to spawn the special prefab(s) here
    }
}
