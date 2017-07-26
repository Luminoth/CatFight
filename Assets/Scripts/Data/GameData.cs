using System;
using System.Collections.Generic;
using System.Text;

using CatFight.Util;

using UnityEngine;

namespace CatFight.Data
{
    // TODO: it would be cool if we had a set of ScriptableObjects
    // to define the data, and then we could export that to JSON for the controller
    [Serializable]
    public sealed class GameData : IData
    {
        public const int CurrentVersion = 1;

        [SerializeField]
        private int version = CurrentVersion;

        public int Version => version;

        public bool IsValid => CurrentVersion == version;

        [SerializeField]
        private FighterData fighter;

        public FighterData Fighter => fighter;

#region Items
        private readonly Dictionary<string, Dictionary<int, ItemData>> _items = new Dictionary<string, Dictionary<int, ItemData>>
        {
            { ItemData.ItemTypeBrain, new Dictionary<int, ItemData>() },
            { ItemData.ItemTypeArmor, new Dictionary<int, ItemData>() },
            { ItemData.ItemTypeWeapon, new Dictionary<int, ItemData>() },
            { ItemData.ItemTypeSpecial, new Dictionary<int, ItemData>() },
        };

        [SerializeField]
        private BrainData[] brains = new BrainData[0];

        [SerializeField]
        private WeaponData[] weapons = new WeaponData[0];

        [SerializeField]
        private ArmorData[] armor = new ArmorData[0];

        [SerializeField]
        private SpecialData[] specials = new SpecialData[0];
#endregion

        public void Process()
        {
            Debug.Log("Processing game data...");

            foreach(BrainData brainData in brains) {
                _items[ItemData.ItemTypeBrain].Add(brainData.Id, brainData);
            }

            foreach(WeaponData weaponData in weapons) {
                _items[ItemData.ItemTypeWeapon].Add(weaponData.Id, weaponData);
            }

            foreach(ArmorData armorData in armor) {
                _items[ItemData.ItemTypeArmor].Add(armorData.Id, armorData);
            }

            foreach(SpecialData specialData in specials) {
                _items[ItemData.ItemTypeSpecial].Add(specialData.Id, specialData);
            }

            Fighter.SchematicData.Process();
        }

        public ItemData GetItem(string itemType, int itemId)
        {
            Dictionary<int, ItemData> itemDatas = _items.GetOrDefault(itemType);
            return itemDatas?.GetOrDefault(itemId);
        }

        public void DebugDump()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Game Data:");

            builder.AppendLine($"Version: {version} ({CurrentVersion}), valid: {IsValid}");

            builder.AppendLine($"Fighter: {fighter}");

            builder.AppendLine("Items:");

            builder.AppendLine($"Brains {brains.Length}:");
            foreach(BrainData brainData in brains) {
                builder.AppendLine(brainData.ToString());
            }

            builder.AppendLine($"Weapons {weapons.Length}:");
            foreach(WeaponData weaponData in weapons) {
                builder.AppendLine(weaponData.ToString());
            }

            builder.AppendLine($"Armor {armor.Length}:");
            foreach(ArmorData armorData in armor) {
                builder.AppendLine(armorData.ToString());
            }

            builder.AppendLine($"Specials {specials.Length}:");
            foreach(SpecialData specialData in specials) {
                builder.AppendLine(specialData.ToString());
            }

            Debug.Log(builder.ToString());
        }
    }
}
