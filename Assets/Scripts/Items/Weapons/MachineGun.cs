using System;

using CatFight.Data;

namespace CatFight.Items.Weapons
{
    [Serializable]
    public sealed class MachineGun : Weapon
    {
        public override WeaponData.WeaponType WeaponType => WeaponData.WeaponType.MachineGun;

        public override void SetStrength(int strength)
        {
UnityEngine.Debug.LogError("TODO: set machinegun strength");
        }

        public override void Fire()
        {
UnityEngine.Debug.LogError("TODO: fire machinegun");
        }
    }
}
