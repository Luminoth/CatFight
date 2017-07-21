using System;

using UnityEngine;

namespace CatFight.Data
{
    [Serializable]
    public sealed class WeaponData : ItemData
    {
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
