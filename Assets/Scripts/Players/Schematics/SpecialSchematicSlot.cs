using System;

using CatFight.Data;
using CatFight.Util;

using JetBrains.Annotations;

namespace CatFight.Players.Schematics
{
    [Serializable]
    public sealed class SpecialSchematicSlot : SchematicSlot
    {
        [CanBeNull]
        public SpecialData.SpecialDataEntry SpecialItem => DataManager.Instance.GameData.Specials.Entries.GetOrDefault(ItemId);

        public SpecialSchematicSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }
    }
}
