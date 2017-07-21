using System;

using CatFight.Items;

using UnityEngine;

namespace CatFight.Data
{
    [Serializable]
    public sealed class WeaponData : ItemData
    {
        public override Item.ItemType ItemType => Item.ItemType.Weapon;

        [SerializeField]
        private string _type = string.Empty;

        public string Type => _type;

        public override void Process()
        {
        }

        public override string ToString()
        {
            return $"Weapon({Id}: {Name})";
        }
    }
}
