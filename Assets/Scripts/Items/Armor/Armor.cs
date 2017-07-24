using UnityEngine;

namespace CatFight.Items.Armor
{
    public static class ArmorFactory
    {
        public static Armor Create(string type)
        {
            switch(type)
            {
            case Armor.ArmorTypeMachineGun:
                return new AntiMachineGun();
            case Armor.ArmorTypeLaser:
                return new AntiLaser();
            default:
                Debug.LogError($"Invalid armor type {type}!");
                return null;
            }
        }
    }

    public abstract class Armor : Item
    {
        public const string ArmorTypeMachineGun = "machinegun";
        public const string ArmorTypeLaser = "laser";

        public abstract string ArmorType { get; }
    }
}
