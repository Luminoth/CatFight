using System;

using CatFight.Data;
using CatFight.Fighters;

namespace CatFight.Items.Weapons
{
    [Serializable]
    public sealed class Laser : Weapon
    {
        public Laser(WeaponData.WeaponDataEntry weaponData)
            : base(weaponData)
        {
        }

        public override void SetStrength(int strength)
        {
UnityEngine.Debug.LogError("TODO: set laser strength");
        }

        protected override void DoFire(Fighter fighter)
        {
UnityEngine.Debug.LogError("TODO: fire laser");
        }
    }
}
