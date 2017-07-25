using System;

using CatFight.Data;
using CatFight.Players.Schematics;

namespace CatFight.Fighters.Loadouts
{
    [Serializable]
    public sealed class SpecialLoadoutSlot : LoadoutSlot
    {
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
        }

        public override void Complete()
        {
        }
    }
}
