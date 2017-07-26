using System;

using UnityEngine;

namespace CatFight.Data
{
    [Serializable]
    public sealed class ArmorData : ItemData
    {
        [SerializeField]
        private string type = string.Empty;

        public string Type => type;

        [SerializeField]
        private int reductionPercent = 0;

        public int ReductionPercent => reductionPercent;

        public override string ToString()
        {
            return $"Armor({Id}: {Name} - {Type} {ReductionPercent})";
        }
    }
}
