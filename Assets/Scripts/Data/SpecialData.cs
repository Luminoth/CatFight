using System;
using System.Collections.Generic;
using System.ComponentModel;

using CatFight.Util;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using UnityEngine;

namespace CatFight.Data
{
    [CreateAssetMenu(fileName="SpecialData", menuName="Cat Fight/Data/Items/Special Data")]
    [Serializable]
    public sealed class SpecialData : ScriptableObject
    {
#region UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/Cat Fight/Data/Items/Special Data")]
        private static void Create()
        {
            ScriptableObjectUtility.CreateAsset<SpecialData>();
        }
#endregion

        public enum SpecialType
        {
            [Description("none")]
            None,

            [Description("missiles")]
            Missiles,

            [Description("chaff")]
            Chaff
        }

        [Serializable]
        public sealed class SpecialDataEntry : ItemData
        {
            [SerializeField]
            private SpecialType _type = SpecialType.None;

            [JsonConverter(typeof(StringEnumConverter))]
            public SpecialType Type => _type;

            public override string ToString()
            {
                return $"Special({Id}: {Name} - {Type})";
            }
        }

        [SerializeField]
        private SpecialDataEntry[] _specials;

        public IReadOnlyCollection<SpecialDataEntry> Specials => _specials;

        private readonly Dictionary<int, SpecialDataEntry> _entries = new Dictionary<int, SpecialDataEntry>();

        [JsonIgnore]
        public IReadOnlyDictionary<int, SpecialDataEntry> Entries => _entries;

        private void Awake()
        {
            foreach(SpecialDataEntry entry in Specials) {
                _entries.Add(entry.Id, entry);
            }
        }
    }
}
