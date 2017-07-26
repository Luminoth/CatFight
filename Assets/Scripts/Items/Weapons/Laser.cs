using System;

using CatFight.Data;

namespace CatFight.Items.Weapons
{
    [Serializable]
    public sealed class Laser : Weapon
    {
        public override WeaponData.WeaponType WeaponType => WeaponData.WeaponType.Laser;

        public override void SetStrength(int strength)
        {
UnityEngine.Debug.LogError("TODO: set laser strength");
        }

        public override void Fire()
        {
UnityEngine.Debug.LogError("TODO: fire laser");
        }
    }
}
