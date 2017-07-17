using System.Collections.Generic;

namespace CatFight.Data
{
    // TODO: it would be cool if we had a set of ScriptableObjects
    // to define the data, and then we could export that to JSON for the controller
    public sealed class GameData
    {
        private readonly Dictionary<string, WeaponData> _weapons = new Dictionary<string, WeaponData>();

        public IReadOnlyDictionary<string, WeaponData> WeaponData => _weapons;

        private readonly Dictionary<string, ArmorData> _armor = new Dictionary<string, ArmorData>();

        public IReadOnlyDictionary<string, ArmorData> ArmorData => _armor;
    }
}
