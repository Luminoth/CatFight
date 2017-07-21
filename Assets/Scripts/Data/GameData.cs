using System;
using System.Collections.Generic;
using System.Text;

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

// TODO: stuff these down into a single dictionary if dictionaries (item type -> (item id -> item))
// and then have a method GetItemData(type, id)

        [SerializeField]
        private BrainData[] brains = new BrainData[0];

        private readonly Dictionary<int, BrainData> _brainData = new Dictionary<int, BrainData>();

        [SerializeField]
        private WeaponData[] weapons = new WeaponData[0];

        private readonly Dictionary<int, WeaponData> _weaponData = new Dictionary<int, WeaponData>();

        [SerializeField]
        private ArmorData[] armor = new ArmorData[0];

        private readonly Dictionary<int, ArmorData> _armorData = new Dictionary<int, ArmorData>();

        [SerializeField]
        private SpecialData[] specials = new SpecialData[0];

        private readonly Dictionary<int, SpecialData> _specialData = new Dictionary<int, SpecialData>();

        public SchematicData schematic = new SchematicData();

        public bool IsValid => CurrentVersion == version;

        public void Process()
        {
            Debug.Log("Processing game data...");

            foreach(BrainData brainData in brains) {
                brainData.Process();
                _brainData.Add(brainData.Id, brainData);
            }

            foreach(WeaponData weaponData in weapons) {
                weaponData.Process();
                _weaponData.Add(weaponData.Id, weaponData);
            }

            foreach(ArmorData armorData in armor) {
                armorData.Process();
                _armorData.Add(armorData.Id, armorData);
            }

            foreach(SpecialData specialData in specials) {
                specialData.Process();
                _specialData.Add(specialData.Id, specialData);
            }


            schematic.Process();
        }

        public void DebugDump()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Game Data:");

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

            builder.AppendLine(schematic.ToString());

            Debug.Log(builder.ToString());
        }
    }
}
