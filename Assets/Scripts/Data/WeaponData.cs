using System;

namespace CatFight.Data
{
    [Serializable]
    public sealed class WeaponData : Data
    {
        public override string ToString()
        {
            return $"Weapon({id}: {name})";
        }
    }
}
