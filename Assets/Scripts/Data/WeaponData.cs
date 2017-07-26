using System;
using System.Collections.Generic;
using System.ComponentModel;

using CatFight.Util;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using UnityEngine;

namespace CatFight.Data
{
    [CreateAssetMenu(fileName="WeaponData", menuName="Cat Fight/Data/Items/Weapon Data")]
    [Serializable]
    public sealed class WeaponData : ScriptableObject
    {
#region UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/Cat Fight/Data/Items/Weapon Data")]
        private static void Create()
        {
            ScriptableObjectUtility.CreateAsset<WeaponData>();
        }
#endregion

        public enum WeaponType
        {
            [Description("none")]
            None,

            [Description("machinegun")]
            MachineGun,

            [Description("laser")]
            Laser
        }

        [Serializable]
        public sealed class WeaponDataEntry : ItemData
        {
            [SerializeField]
            private WeaponType _type = WeaponType.None;

            [JsonConverter(typeof(StringEnumConverter))]
            public WeaponType Type => _type;

            [SerializeField]
            [Range(0, 100)]
            private int _damage;

            [JsonIgnore]
            public int Damage => _damage;

            public override string ToString()
            {
                return $"Weapon({Id}: {Name} - {Type}, {Damage})";
            }
        }

        [SerializeField]
        private WeaponDataEntry[] _weapons;

        public IReadOnlyCollection<WeaponDataEntry> Weapons => _weapons;

        private readonly Dictionary<int, WeaponDataEntry> _entries = new Dictionary<int, WeaponDataEntry>();

        [JsonIgnore]
        public IReadOnlyDictionary<int, WeaponDataEntry> Entries => _entries;

        private void Awake()
        {
            foreach(WeaponDataEntry entry in Weapons) {
                _entries.Add(entry.Id, entry);
            }
        }
    }
}
