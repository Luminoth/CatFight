using System;

using CatFight.Data;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Items.Weapons
{
    public static class WeaponFactory
    {
        [CanBeNull]
        public static Weapon Create(WeaponData.WeaponDataEntry weaponData)
        {
            switch(weaponData.Type)
            {
            case WeaponData.WeaponType.MachineGun:
                return new MachineGun(weaponData);
            case WeaponData.WeaponType.Laser:
                return new Laser(weaponData);
            default:
                Debug.LogError($"Invalid weapon type {weaponData.Type}!");
                return null;
            }
        }
    }

    [Serializable]
    public abstract class Weapon : Item
    {
        public WeaponData.WeaponType WeaponType => _weaponData.Type;

#region Cooldown
        public DateTime _cooldownEndTime = DateTime.Now;

        public bool IsOnCooldown => _cooldownEndTime > DateTime.Now;
#endregion

        private readonly WeaponData.WeaponDataEntry _weaponData;

        public TimeSpan GetCooldownRemaining()
        {
            DateTime now = DateTime.Now;
            if(_cooldownEndTime < now) {
                return TimeSpan.Zero;
            }
            return _cooldownEndTime - now;
        }

        public abstract void SetStrength(int strength);

        public void Fire()
        {
            if(IsOnCooldown) {
                return;
            }

            DoFire();

            _cooldownEndTime = DateTime.Now.AddSeconds(_weaponData.CooldownSeconds);
        }

        protected abstract void DoFire();

        protected Weapon(WeaponData.WeaponDataEntry weaponData)
        {
            _weaponData = weaponData;
        }
    }
}
