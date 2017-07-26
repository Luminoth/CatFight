using System;

using CatFight.Data;
using CatFight.Util;

using JetBrains.Annotations;

namespace CatFight.Players.Schematics
{
    [Serializable]
    public sealed class WeaponSchematicSlot : SchematicSlot
    {
        [CanBeNull]
        public WeaponData.WeaponDataEntry WeaponItem => DataManager.Instance.GameData.Weapons.Entries.GetOrDefault(ItemId);

        public WeaponSchematicSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }
    }
}
