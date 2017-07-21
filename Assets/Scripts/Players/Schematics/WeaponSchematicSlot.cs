using System;

using CatFight.Data;

using JetBrains.Annotations;

namespace CatFight.Players.Schematics
{
    [Serializable]
    public sealed class WeaponSchematicSlot : SchematicSlot
    {
        [CanBeNull]
        public WeaponData WeaponItem => (WeaponData)Item;

        public WeaponSchematicSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }
    }
}
