using System;

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

    [Serializable]
    public abstract class Special : Item
    {
        public const string SpecialTypeMissiles = "missiles";
        public const string SpecialTypeChaff = "chaff";

        public abstract string SpecialType { get; }

        [SerializeField]
        private int _totalUses;

        public int TotalUses { get { return _totalUses; } private set { _totalUses = value; } }

        [SerializeField]
        private int _remainingUses;

        public int RemainingUses { get { return _remainingUses; } private set { _remainingUses = value; } }

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
