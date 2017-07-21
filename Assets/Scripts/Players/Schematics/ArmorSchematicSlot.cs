using CatFight.Data;

namespace CatFight.Players.Schematics
{
    public sealed class ArmorSchematicSlot : SchematicSlot
    {
        public ArmorData ArmorItem => (ArmorData)Item;

        public ArmorSchematicSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }
    }
}
