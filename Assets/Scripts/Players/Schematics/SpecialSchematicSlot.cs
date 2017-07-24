using System;

using CatFight.Data;

using JetBrains.Annotations;

namespace CatFight.Players.Schematics
{
    [Serializable]
    public sealed class SpecialSchematicSlot : SchematicSlot
    {
        [CanBeNull]
        public SpecialData SpecialItem => (SpecialData)Item;

        public SpecialSchematicSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }
    }
}
