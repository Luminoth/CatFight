using System;

using CatFight.Data;

using JetBrains.Annotations;

namespace CatFight.Players.Schematics
{
    [Serializable]
    public sealed class BrainSchematicSlot : SchematicSlot
    {
        [CanBeNull]
        public BrainData BrainItem => (BrainData)Item;

        public BrainSchematicSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }
    }
}
