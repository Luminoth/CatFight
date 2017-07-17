using System;

namespace CatFight.Data
{
    [Serializable]
    public sealed class WeaponData : Data
    {
        public override void Process()
        {
        }

        public override string ToString()
        {
            return $"Weapon({Id}: {Name})";
        }
    }
}
