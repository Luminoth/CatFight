using System;

using CatFight.Data;
using CatFight.Util;

using JetBrains.Annotations;

namespace CatFight.Players.Schematics
{
    [Serializable]
    public sealed class ArmorSchematicSlot : SchematicSlot
    {
        [CanBeNull]
        public ArmorData.ArmorDataEntry ArmorItem => DataManager.Instance.GameData.Armor.Entries.GetOrDefault(ItemId);

        public ArmorSchematicSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }
    }
}
