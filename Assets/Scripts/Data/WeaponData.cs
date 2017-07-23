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

        public override void Process()
        {
        }

        public override string ToString()
        {
            return $"Weapon({Id}: {Name} - {Type})";
        }
    }
}
