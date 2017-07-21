using CatFight.Data;

namespace CatFight.Players.Schematics
{
    public sealed class BrainSchematicSlot : SchematicSlot
    {
        public BrainData BrainItem => (BrainData)Item;

        public BrainSchematicSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }
    }
}
