using System;
using System.Text;

using UnityEngine;

namespace CatFight.Data
{
    [Serializable]
    public sealed class FighterData : IData
    {
        [SerializeField]
        private int maxHealth;

        public int MaxHealth => maxHealth;

        [SerializeField]
        private int moveSpeed;

        public int MoveSpeed => moveSpeed;

        [SerializeField]
        private int armorReductionCapPercent = 50;

        public int ArmorReductionCapPercent => armorReductionCapPercent;

        [SerializeField]
        private SchematicData schematic = new SchematicData();

        public SchematicData SchematicData => schematic;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Fighter(maxHealth: {maxHealth}, moveSpeed: {moveSpeed})");

            builder.AppendLine("Schematics:");
            builder.AppendLine(SchematicData.ToString());

            return builder.ToString();
        }
    }
}
