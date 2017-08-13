using System;

using CatFight.Data;
using CatFight.Fighters;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Items.Weapons
{
    public static class WeaponFactory
    {
        [CanBeNull]
        public static Weapon Create(Fighter fighter, WeaponData.WeaponDataEntry weaponData)
        {
            switch(weaponData.Type)
            {
            case WeaponData.WeaponType.MachineGun:
                return new MachineGun(fighter, weaponData);
            case WeaponData.WeaponType.Laser:
                return new Laser(fighter, weaponData);
            default:
                Debug.LogError($"Invalid weapon type {weaponData.Type}!");
                return null;
            }
        }
    }

    [Serializable]
    public abstract class Weapon : Item
    {
        public WeaponData.WeaponType WeaponType => WeaponData.Type;

#region Cooldown
        public DateTime _cooldownEndTime = DateTime.Now;

        public bool IsOnCooldown => _cooldownEndTime > DateTime.Now;
#endregion

        public WeaponData.WeaponDataEntry WeaponData { get; }

        public Fighter Fighter { get; }

        private int _strength;

        public int Strength
        {
            get { return _strength; }

            set { _strength = value < 1 ? 1 : value; }
        }

        public int Damage => WeaponData.Damage + Strength;

        public TimeSpan GetCooldownRemaining()
        {
            DateTime now = DateTime.Now;
            if(_cooldownEndTime < now) {
                return TimeSpan.Zero;
            }
            return _cooldownEndTime - now;
        }

        public void Fire()
        {
            if(IsOnCooldown) {
                return;
            }

            DoFire();

            _cooldownEndTime = DateTime.Now.AddMilliseconds(WeaponData.CooldownMilliseconds);
        }

        protected abstract void DoFire();

        protected Weapon(Fighter fighter, WeaponData.WeaponDataEntry weaponData)
        {
            Fighter = fighter;
            WeaponData = weaponData;
        }
    }
}
