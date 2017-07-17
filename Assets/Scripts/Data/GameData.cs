using System;
using System.Text;

using UnityEngine;

namespace CatFight.Data
{
    // TODO: it would be cool if we had a set of ScriptableObjects
    // to define the data, and then we could export that to JSON for the controller
    [Serializable]
    public sealed class GameData
    {
        public WeaponData[] weapons = new WeaponData[0];

        public ArmorData[] armor = new ArmorData[0];

        public void DebugDump()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Game Data:");

            builder.AppendLine($"Weapons {weapons.Length}:");
            foreach(WeaponData weaponData in weapons) {
                builder.AppendLine(weaponData.ToString());
            }

            builder.AppendLine($"Armor {armor.Length}:");
            foreach(ArmorData armorData in armor) {
                builder.AppendLine(armorData.ToString());
            }

            Debug.Log(builder.ToString());
        }
    }
}
