using System;
using System.Collections.Generic;

using CatFight.Data;
using CatFight.Util;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Items.Specials
{
    public static class SpecialFactory
    {
        [CanBeNull]
        public static Special Create(SpecialData.SpecialDataEntry specialData, int uses=0)
        {
            switch(specialData.Type)
            {
            case SpecialData.SpecialType.Missiles:
                return new Missiles(specialData, uses);
            case SpecialData.SpecialType.Chaff:
                return new Chaff(specialData, uses);
            default:
                Debug.LogError($"Invalid special type {specialData.Type}!");
                return null;
            }
        }

        public static void Init(IDictionary<SpecialData.SpecialType, Special> specials, IList<Special> specialList)
        {
            foreach(SpecialData.SpecialDataEntry specialData in DataManager.Instance.GameData.Specials.Specials) {
                Special special = Create(specialData);

                specials.Add(specialData.Type, special);
                specialList.Add(special);
            }
        }
    }

    [Serializable]
    public abstract class Special : Item
    {
        public SpecialData.SpecialType SpecialType => _specialData.Type;

        [SerializeField]
        [ReadOnly]
        private int _totalUses;

        public int TotalUses { get { return _totalUses; } private set { _totalUses = value; } }

        [SerializeField]
        [ReadOnly]
        private int _remainingUses;

        public int RemainingUses { get { return _remainingUses; } private set { _remainingUses = value; } }

#region Cooldown
        public DateTime _cooldownEndTime = DateTime.Now;

        public bool IsOnCooldown => _cooldownEndTime > DateTime.Now;
#endregion

        private readonly SpecialData.SpecialDataEntry _specialData;

        public TimeSpan GetCooldownRemaining()
        {
            DateTime now = DateTime.Now;
            if(_cooldownEndTime < now) {
                return TimeSpan.Zero;
            }
            return _cooldownEndTime - now;
        }

        public void IncreaseTotalUses(int amount, bool increaseRemaining=true)
        {
            TotalUses += amount;
            if(increaseRemaining) {
                RemainingUses += amount;
            }
        }

        public void Use()
        {
            if(RemainingUses <= 0 || IsOnCooldown) {
                return;
            }

            DoUse();

            --RemainingUses;

            _cooldownEndTime = DateTime.Now.AddSeconds(_specialData.CooldownSeconds);
        }

        protected abstract void DoUse();

        protected Special(SpecialData.SpecialDataEntry specialData, int totalUses)
        {
            _specialData = specialData;
            TotalUses = totalUses;
            RemainingUses = TotalUses;
        }
    }
}
