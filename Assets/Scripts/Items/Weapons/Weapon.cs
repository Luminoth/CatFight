using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Items.Weapons
{
    public static class WeaponFactory
    {
        [CanBeNull]
        public static Weapon Create(string type)
        {
            switch(type)
            {
            case Weapon.WeaponTypeMachineGun:
                return new MachineGun();
            case Weapon.WeaponTypeLaser:
                return new Laser();
            default:
                Debug.LogError($"Invalid weapon type {type}!");
                return null;
            }
        }
    }

    public abstract class Weapon : Item
    {
        public const string WeaponTypeMachineGun = "machinegun";
        public const string WeaponTypeLaser = "laser";

        public abstract string WeaponType { get; }

        public abstract void SetStrength(int strength);

        public abstract void Fire();
    }
}
