using System;
using System.Collections.Generic;

using CatFight.Util;

using Newtonsoft.Json;

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

        [Serializable]
        public sealed class BrainDataEntry : ItemData
        {
            [SerializeField]
            private FsmTemplate _template;

            [JsonIgnore]
            public FsmTemplate Template => _template;

            public override string ToString()
            {
                return $"Brain({Id}: {Name})";
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
