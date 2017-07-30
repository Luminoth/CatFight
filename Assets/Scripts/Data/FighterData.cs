using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CatFight.Util;

using Newtonsoft.Json;

using UnityEngine;

namespace CatFight.Data
{
    [CreateAssetMenu(fileName="FighterData", menuName="Cat Fight/Data/Fighter Data")]
    [Serializable]
    public sealed class FighterData : ScriptableObject
    {
#region UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/Cat Fight/Data/Items/Brain Data")]
        private static void Create()
        {
            ScriptableObjectUtility.CreateAsset<FighterData>();
        }
#endregion

        private const string DefaultFighterName = "Missing Name";

        [SerializeField]
        [Range(0, 500)]
        private int _maxHealth = 100;

        [JsonIgnore]
        public int MaxHealth => _maxHealth;

        [SerializeField]
        [Range(0, 100)]
        private int _moveSpeed = 10;

        [JsonIgnore]
        public int MoveSpeed => _moveSpeed;

        [SerializeField]
        [Range(0, 100)]
        private int _armorReductionCapPercent = 50;

        [JsonIgnore]
        public int ArmorReductionCapPercent => _armorReductionCapPercent;

        [SerializeField]
        private SchematicData _schematic = new SchematicData();

        public SchematicData Schematic => _schematic;

        [SerializeField]
        private string[] _fighterNames;

        [JsonIgnore]
        public IReadOnlyCollection<string> FighterNames => _fighterNames;

        public void GetRandomFighterNames(IList<string> names, int count)
        {
// TODO: make this actually random
            int i;
            for(i=0; i<FighterNames.Count && i<count; ++i) {
                names.Add(FighterNames.ElementAt(i));
            }

            for(; i<count; ++i) {
                names.Add(DefaultFighterName);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Fighter(maxHealth: {MaxHealth}, moveSpeed: {MoveSpeed})");

            builder.AppendLine("Schematics:");
            builder.AppendLine(Schematic.ToString());

            return builder.ToString();
        }
    }
}
