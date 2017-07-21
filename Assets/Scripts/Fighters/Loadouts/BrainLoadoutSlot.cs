using System;

using CatFight.Data;
using CatFight.Players.Schematics;

namespace CatFight.Fighters.Loadouts
{
    [Serializable]
    public sealed class BrainLoadoutSlot : LoadoutSlot
    {
        public BrainLoadoutSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }

        public override void Process(SchematicSlot schematicSlot)
        {
            BrainSchematicSlot brainSlot = (BrainSchematicSlot)schematicSlot;
            if(null == brainSlot.Item) {
                return;
            }
        }

        public override void Complete()
        {
        }
    }
}
