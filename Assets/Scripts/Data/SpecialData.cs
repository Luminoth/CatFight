using System;
using System.Collections.Generic;
using System.ComponentModel;

using CatFight.Items.Specials;
using CatFight.Items.Weapons;
using CatFight.Util;
using CatFight.Util.ObjectPool;

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

// TODO: these should be extensions of SpecialType
        public static string GetAmmoPool(SpecialType specialType)
        {
            return specialType.GetDescription() + "-ammo";
        }

        public static string GetImpactPool(SpecialType specialType)
        {
            return specialType.GetDescription() + "-impact";
        }

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

            [SerializeField]
            private int _spawnAmount = 1;

            [JsonIgnore]
            public int SpawnAmount => _spawnAmount;

            [SerializeField]
            private int _spawnRateMs = 500;

            [JsonIgnore]
            public int SpawnRateMs => _spawnRateMs;

            [JsonIgnore]
            public float SpawnRateSeconds => _spawnRateMs / 1000.0f;

            [SerializeField]
            [Range(0, 100)]
            private int _damage;

            [JsonIgnore]
            public int Damage => _damage;

            [SerializeField]
            [Range(0, 60)]
            private int _cooldownSeconds;

            [JsonIgnore]
            public int CooldownSeconds => _cooldownSeconds;
            
            public int ActualCooldownSeconds => Mathf.CeilToInt(SpawnRateSeconds * _spawnAmount) + _cooldownSeconds;

            [SerializeField]
            private SpecialAmmo _ammoPrefab;

            [JsonIgnore]
            public SpecialAmmo AmmoPrefab => _ammoPrefab;

            [SerializeField]
            private int _poolSize;

            [JsonIgnore]
            public int PoolSize => _poolSize;

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

        public void Initialize()
        {
            foreach(SpecialDataEntry entry in Specials) {
                _entries.Add(entry.Id, entry);
                if(null != entry.AmmoPrefab) {
                    PoolAmmo(entry.Type, entry.AmmoPrefab, entry.PoolSize);
                }
            }
        }

        private void PoolAmmo(SpecialType specialType, SpecialAmmo ammo, int poolSize)
        {
            PooledObject pooledObject = ammo?.GetComponent<PooledObject>();
            if(null != pooledObject) {
                ObjectPoolManager.Instance.InitializePool(GetAmmoPool(specialType), pooledObject, poolSize);
            }

            if(null != ammo.ImpactPrefab) {
                PoolImpact(specialType, ammo.ImpactPrefab, poolSize);
            }
        }

        private void PoolImpact(SpecialType specialType, Impact impact, int poolSize)
        {
            PooledObject pooledObject = impact?.GetComponent<PooledObject>();
            if(null != pooledObject) {
                ObjectPoolManager.Instance.InitializePool(GetImpactPool(specialType), pooledObject, poolSize);
            }
        }
    }
}
