using System;

using UnityEngine;

namespace CatFight.Data
{
    [Serializable]
    public sealed class ArmorData : ItemData
    {
        [SerializeField]
        private int _slots = 1;

        public int Slots => _slots;

        [SerializeField]
        private int moveModPercent = 0;

        public int MoveModifierPercent => moveModPercent;

        public float MoveModifier => moveModPercent / 100.0f;

        [SerializeField]
        private int armorReductionPercent = 0;

        public int ArmorReductionPercent => armorReductionPercent;

        public float ArmorReduction => armorReductionPercent / 100.0f;

        public override void Process()
        {
        }

        public override string ToString()
        {
            return $"Armor({Id}: {Name})";
        }
    }
}
