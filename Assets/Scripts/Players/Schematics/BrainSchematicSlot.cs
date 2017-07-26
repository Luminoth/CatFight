using System;

using CatFight.Data;
using CatFight.Util;

using JetBrains.Annotations;

namespace CatFight.Players.Schematics
{
    [Serializable]
    public sealed class BrainSchematicSlot : SchematicSlot
    {
        [CanBeNull]
        public BrainData.BrainDataEntry BrainItem => DataManager.Instance.GameData.Brains.Entries.GetOrDefault(ItemId);

        public BrainSchematicSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }
    }
}
