using System;
using System.Collections.Generic;
using System.ComponentModel;

using CatFight.Util;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using UnityEngine;

namespace CatFight.Data
{
    [CreateAssetMenu(fileName="BrainData", menuName="Cat Fight/Data/Items/Brain Data")]
    [Serializable]
    public sealed class BrainData : ScriptableObject
    {
#region UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/Cat Fight/Data/Items/Brain Data")]
        private static void Create()
        {
            ScriptableObjectUtility.CreateAsset<BrainData>();
        }
#endregion

        public enum BrainType
        {
            [Description("none")]
            None,

            [Description("aggressive")]
            Aggressive,

            [Description("defensive")]
            Defensive
        }

        [Serializable]
        public sealed class BrainDataEntry : ItemData
        {
            [SerializeField]
            private BrainType _type = BrainType.None;

            [JsonConverter(typeof(StringEnumConverter))]
            public BrainType Type => _type;

            public override string ToString()
            {
                return $"Brain({Id}: {Name} - {Type})";
            }
        }

        [SerializeField]
        private BrainDataEntry[] _brains;

        public IReadOnlyCollection<BrainDataEntry> Brains => _brains;

        private readonly Dictionary<int, BrainDataEntry> _entries = new Dictionary<int, BrainDataEntry>();

        [JsonIgnore]
        public IReadOnlyDictionary<int, BrainDataEntry> Entries => _entries;

        public void Initialize()
        {
            foreach(BrainDataEntry entry in Brains) {
                _entries.Add(entry.Id, entry);
            }
        }
    }
}
