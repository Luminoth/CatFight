using CatFight.Data;

namespace CatFight.Players.Schematics
{
    public sealed class SpecialSchematicSlot : SchematicSlot
    {
        public SpecialData SpecialItem => (SpecialData)Item;

        public SpecialSchematicSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }
    }
}
