using System;

using CatFight.Data;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Items.Weapons
{
    public static class WeaponFactory
    {
        [CanBeNull]
        public static Weapon Create(WeaponData.WeaponType type)
        {
            switch(type)
            {
            case WeaponData.WeaponType.MachineGun:
                return new MachineGun();
            case WeaponData.WeaponType.Laser:
                return new Laser();
            default:
                Debug.LogError($"Invalid weapon type {type}!");
                return null;
            }
        }
    }

    [Serializable]
    public abstract class Weapon : Item
    {
        public abstract WeaponData.WeaponType WeaponType { get; }

        public abstract void SetStrength(int strength);

        public abstract void Fire();
    }
}
