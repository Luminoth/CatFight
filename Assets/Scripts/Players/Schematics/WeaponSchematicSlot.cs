using CatFight.Data;

namespace CatFight.Players.Schematics
{
    public sealed class WeaponSchematicSlot : SchematicSlot
    {
        public WeaponData WeaponItem => (WeaponData)Item;

        public WeaponSchematicSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }
    }
}
