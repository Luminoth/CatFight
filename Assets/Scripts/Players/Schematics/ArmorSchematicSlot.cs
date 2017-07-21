using System;

using CatFight.Data;

using JetBrains.Annotations;

namespace CatFight.Players.Schematics
{
    [Serializable]
    public sealed class ArmorSchematicSlot : SchematicSlot
    {
        [CanBeNull]
        public ArmorData ArmorItem => (ArmorData)Item;

        public ArmorSchematicSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }
    }
}
