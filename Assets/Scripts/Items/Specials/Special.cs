using System;

using CatFight.Data;
using CatFight.Util;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Items.Specials
{
    public static class SpecialFactory
    {
        [CanBeNull]
        public static Special Create(SpecialData.SpecialType type, int uses)
        {
            switch(type)
            {
            case SpecialData.SpecialType.Missiles:
                return new Missiles(uses);
            case SpecialData.SpecialType.Chaff:
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
        public abstract SpecialData.SpecialType SpecialType { get; }

        [SerializeField]
        [ReadOnly]
        private int _totalUses;

        public int TotalUses { get { return _totalUses; } private set { _totalUses = value; } }

        [SerializeField]
        [ReadOnly]
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
