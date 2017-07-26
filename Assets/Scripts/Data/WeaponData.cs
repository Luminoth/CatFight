using System;

using UnityEngine;

namespace CatFight.Data
{
    [Serializable]
    public sealed class WeaponData : ItemData
    {
        [SerializeField]
        private string type = string.Empty;

        public string Type => type;

        [SerializeField]
        private int damage = 1;

        public int Damage => damage;

        public override string ToString()
        {
            return $"Weapon({Id}: {Name} - {Type}, {Damage})";
        }
    }
}
