using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Items.Specials
{
    public static class SpecialFactory
    {
        [CanBeNull]
        public static Special Create(string type, int uses)
        {
            switch(type)
            {
            case Special.SpecialTypeMissiles:
                return new Missiles(uses);
            case Special.SpecialTypeChaff:
                return new Chaff(uses);
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

        public int TotalUses { get; private set; }

        public int RemainingUses { get; private set; }

        public void IncreaseTotalUses(int amount, bool increaseRemaining=true)
        {
            TotalUses += amount;
            if(increaseRemaining) {
                RemainingUses += amount;
            }
        }

        public void Use()
        {
            if(RemainingUses <= 0) {
                return;
            }

            DoUse();

            --RemainingUses;
        }

        protected abstract void DoUse();

        protected Special(int totalUses)
        {
            TotalUses = totalUses;
            RemainingUses = TotalUses;
        }
    }
}
