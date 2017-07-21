using System;

using CatFight.Items;

using UnityEngine;

namespace CatFight.Data
{
    [Serializable]
    public sealed class BrainData : ItemData
    {
        public override Item.ItemType ItemType => Item.ItemType.Brain;

        [SerializeField]
        private string _type = string.Empty;

        public string Type => _type;

        public override void Process()
        {
        }

        public override string ToString()
        {
            return $"Brain({Id}: {Name})";
        }
    }
}
