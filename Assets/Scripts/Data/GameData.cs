using System;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace CatFight.Data
{
    [CreateAssetMenu(fileName="GameData", menuName="Cat Fight/Data/Game Data")]
    [Serializable]
    public sealed class GameData : ScriptableObject
    {
        // TODO: this sucks
#region Version
        public const int CurrentVersion = 1;

        [SerializeField]
        private int _version = 1;

        public int Version => _version;

        [JsonIgnore]
        public bool IsValid => CurrentVersion == _version;
#endregion

        [SerializeField]
        private FighterData _fighter;

        public FighterData Fighter => _fighter;

#region Items
        [SerializeField]
        private BrainData _brains;

        public BrainData Brains => _brains;

        [SerializeField]
        private WeaponData _weapons;

        public WeaponData Weapons => _weapons;

        [SerializeField]
        private ArmorData _armor;

        public ArmorData Armor => _armor;

        [SerializeField]
        private SpecialData _specials;

        public SpecialData Specials => _specials;
#endregion

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void DebugDump()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Game Data:");

            builder.AppendLine($"Version: {Version} ({CurrentVersion}), valid: {IsValid}");

            builder.AppendLine(Fighter.ToString());
// TODO: fix this
/*
            builder.AppendLine("Items:");

            builder.AppendLine($"Brains {Brains.Count}:");
            foreach(BrainData brainData in Brains) {
                builder.AppendLine(brainData.ToString());
            }

            builder.AppendLine($"Weapons {Weapons.Count}:");
            foreach(WeaponData weaponData in Weapons) {
                builder.AppendLine(weaponData.ToString());
            }

            builder.AppendLine($"Armor {Armor.Count}:");
            foreach(ArmorData armorData in Armor) {
                builder.AppendLine(armorData.ToString());
            }

            builder.AppendLine($"Specials {Specials.Count}:");
            foreach(SpecialData specialData in Specials) {
                builder.AppendLine(specialData.ToString());
            }
*/
            Debug.Log(builder.ToString());
        }
    }
}
