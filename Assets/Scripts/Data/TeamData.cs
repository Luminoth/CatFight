using System;
using System.Collections.Generic;

using CatFight.Util;

using Newtonsoft.Json;

using UnityEngine;

namespace CatFight.Data
{
    [CreateAssetMenu(fileName="TeamData", menuName="Cat Fight/Data/Team Data")]
    [Serializable]
    public sealed class TeamData : ScriptableObject
    {
#region UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/Cat Fight/Data/Team Data")]
        private static void Create()
        {
            ScriptableObjectUtility.CreateAsset<TeamData>();
        }
#endregion

        [Serializable]
        public sealed class TeamDataEntry
        {
            [SerializeField]
            private int _id;

            public int Id => _id;

            [SerializeField]
            private LayerMask _layer;

            [JsonIgnore]
            public LayerMask Layer => (int)Mathf.Log(_layer.value, 2);

            public string Name => LayerMask.LayerToName(Layer);

            public override string ToString()
            {
                return $"Team({Id}: {Name})";
            }
        }

        [SerializeField]
        private TeamDataEntry[] _teams;

        public IReadOnlyCollection<TeamDataEntry> Teams => _teams;

        private readonly Dictionary<int, TeamDataEntry> _entries = new Dictionary<int, TeamDataEntry>();

        [JsonIgnore]
        public IReadOnlyDictionary<int, TeamDataEntry> Entries => _entries;

        public void Initialize()
        {
            foreach(TeamDataEntry entry in Teams) {
                _entries.Add(entry.Id, entry);
            }
        }
    }
}
