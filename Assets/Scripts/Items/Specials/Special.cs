using UnityEngine;

namespace CatFight.Items.Specials
{
    public static class SpecialFactory
    {
        public static Special Create(string type)
        {
            switch(type)
            {
            case Special.SpecialTypeMissiles:
                return new Missiles();
            case Special.SpecialTypeChaff:
                return new Chaff();
            default:
                Debug.LogError($"Invalid special type {type}!");
                return null;
            }
        }
    }

    public abstract class Special : Item
    {
        public const string SpecialTypeMissiles = "missiles";
        public const string SpecialTypeChaff = "chaff";

        public abstract string SpecialType { get; }
    }
}
