using System;
using System.Collections.Generic;

using CatFight.Util;

using Newtonsoft.Json;

using UnityEngine;

namespace CatFight.Data
{
    [CreateAssetMenu(fileName="AudioData", menuName="Cat Fight/Data/Audio Data")]
    [Serializable]
    public sealed class AudioData : ScriptableObject
    {
#region UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/Cat Fight/Data/Audio Data")]
        private static void Create()
        {
            ScriptableObjectUtility.CreateAsset<AudioData>();
        }
#endregion

        [Serializable]
        public sealed class AudioDataEntry
        {
            [SerializeField]
            private string _id;

            public string Id => _id;

            [SerializeField]
            private AudioClip _audioClip;

            [JsonIgnore]
            public AudioClip AudioClip => _audioClip;

            public override string ToString()
            {
                return $"Armor({Id}: {_audioClip.name})";
            }
        }

        [SerializeField]
        private AudioDataEntry[] _audio;

        [JsonIgnore]
        public IReadOnlyCollection<AudioDataEntry> Audio => _audio;

        private readonly Dictionary<string, AudioDataEntry> _entries = new Dictionary<string, AudioDataEntry>();

        [JsonIgnore]
        public IReadOnlyDictionary<string, AudioDataEntry> Entries => _entries;

        public void Initialize()
        {
            foreach(AudioDataEntry entry in Audio) {
                _entries.Add(entry.Id, entry);
            }
        }
    }
}
