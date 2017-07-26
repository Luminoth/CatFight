using System;
using System.Collections.Generic;

using CatFight.Util;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using UnityEngine;

namespace CatFight.Data
{
    [CreateAssetMenu(fileName="ArmorData", menuName="Cat Fight/Data/Items/Armor Data")]
    [Serializable]
    public sealed class ArmorData : ScriptableObject
    {
#region UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/Cat Fight/Data/Items/Armor Data")]
        private static void Create()
        {
            ScriptableObjectUtility.CreateAsset<ArmorData>();
        }
#endregion

        [Serializable]
        public sealed class ArmorDataEntry : ItemData
        {
            [SerializeField]
            private WeaponData.WeaponType _type = WeaponData.WeaponType.None;

            [JsonConverter(typeof(StringEnumConverter))]
            public WeaponData.WeaponType Type => _type;

            [SerializeField]
            [Range(0, 100)]
            private int _reductionPercent = 0;

            public int ReductionPercent => _reductionPercent;

            public override string ToString()
            {
                return $"Armor({Id}: {Name} - {Type} {ReductionPercent})";
            }
        }

        [SerializeField]
        private ArmorDataEntry[] _armor;

        public IReadOnlyCollection<ArmorDataEntry> Armor => _armor;

        private readonly Dictionary<int, ArmorDataEntry> _entries = new Dictionary<int, ArmorDataEntry>();

        [JsonIgnore]
        public IReadOnlyDictionary<int, ArmorDataEntry> Entries => _entries;

        public void Initialize()
        {
            foreach(ArmorDataEntry entry in Armor) {
                _entries.Add(entry.Id, entry);
            }
        }
    }
}
