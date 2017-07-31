using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CatFight.Data;
using CatFight.Fighters.Loadouts;
using CatFight.Items.Armor;
using CatFight.Items.Brains;
using CatFight.Items.Specials;
using CatFight.Items.Weapons;
using CatFight.Stages;
using CatFight.Util;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Fighters
{
    [Serializable]
    public sealed class FighterStats
    {
// TODO: this should be configurable on a prefab somewhere
        private const int MinimumDamage = 1;

#region Health
        [SerializeField]
        [ReadOnly]
        private float _currentHealth = 100.0f;

        public float CurrentHealth { get { return _currentHealth; } private set { _currentHealth = value < 0.0f ? 0.0f : value; } }

        public bool IsDead => CurrentHealth <= 0.0f;
#endregion

#region Brain
        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private Brain _brain;

        [CanBeNull]
        public Brain Brain => _brain;
#endregion

#region Armor
        [SerializeField]
        [ReadOnly]
        private Armor _armor = new Armor();

        public Armor Armor => _armor;
#endregion

#region Weapons
        private readonly List<Weapon> _weapons = new List<Weapon>();

        public IReadOnlyCollection<Weapon> Weapons => _weapons;
#endregion

#region Specials
        private readonly Dictionary<SpecialData.SpecialType, Special> _specials = new Dictionary<SpecialData.SpecialType, Special>();

        public IReadOnlyDictionary<SpecialData.SpecialType, Special> Specials => _specials;
#endregion

#region Movement
        public float MoveSpeed => DataManager.Instance.GameData.Fighter.MoveSpeed;
#endregion

        private readonly Fighter _fighter;

        public FighterStats(Fighter fighter)
        {
            _fighter = fighter;
        }

#region Initialization
        public void Initialize(Loadout loadout, PlayMakerFSM fsm)
        {
            CurrentHealth = DataManager.Instance.GameData.Fighter.MaxHealth;

            _brain = null;
            _armor = new Armor();
            _weapons.Clear();

            _specials.Clear();
            SpecialFactory.Init(_specials);

            foreach(var kvp in loadout.Slots) {
                LoadoutSlot slot = kvp.Value;
                switch(slot.SlotData.Type)
                {
                case SchematicSlotData.SlotType.Brain:
                    SetBrain((BrainLoadoutSlot)slot, fsm);
                    break;
                case SchematicSlotData.SlotType.Weapon:
                    AddWeapon((WeaponLoadoutSlot)slot);
                    break;
                case SchematicSlotData.SlotType.Armor:
                    IncreaseArmorStrength((ArmorLoadoutSlot)slot);
                    break;
                case SchematicSlotData.SlotType.Special:
                    IncreaseSpecialUses((SpecialLoadoutSlot)slot);
                    break;
                default:
                    Debug.LogError($"Invalid loadout slot type {slot.SlotData.Type}!");
                    break;
                }
            }
        }

        private void SetBrain(BrainLoadoutSlot brainSlot, PlayMakerFSM fsm)
        {
            if(null != brainSlot.Brain) {
                _brain = brainSlot.Brain;
                fsm.SetFsmTemplate(Brain?.Template);
            }
        }

        private void AddWeapon(WeaponLoadoutSlot weaponSlot)
        {
            if(null != weaponSlot.Weapon) {
                _weapons.Add(weaponSlot.Weapon);
            }
        }

        private void IncreaseArmorStrength(ArmorLoadoutSlot armorSlot)
        {
            foreach(var kvp in armorSlot.ArmorTypeVotes) {
                ArmorData.ArmorDataEntry armorData = DataManager.Instance.GameData.Armor.Entries.GetOrDefault(kvp.Key);
                if(null == armorData) {
                    continue;
                }

                _armor.IncreaseStrength(armorData.Type, kvp.Value, armorData.ReductionPercent, DataManager.Instance.GameData.Fighter.ArmorReductionCapPercent);
            }
        }

        private void IncreaseSpecialUses(SpecialLoadoutSlot specialSlot)
        {
            foreach(var kvp in specialSlot.Specials) {
                SpecialData.SpecialDataEntry specialData = DataManager.Instance.GameData.Specials.Entries.GetOrDefault(kvp.Key);
                if(null == specialData) {
                    continue;
                }

                Special special = _specials.GetOrDefault(specialData.Type);
                if(null == special) {
                    special = SpecialFactory.Create(specialData, kvp.Value);
                    _specials.Add(specialData.Type, special);
                } else {
                    special.IncreaseTotalUses(kvp.Value);
                }
            }
        }
#endregion

        public void Damage(int amount, WeaponData.WeaponType type)
        {
            if(!GameStageManager.Instance.IsGameStarted) {
                return;
            }

            float reducedAmount = amount - (amount * _armor.GetDamageReduction(type));
            if(reducedAmount < MinimumDamage) {
                reducedAmount = MinimumDamage;
            }
            CurrentHealth -= reducedAmount;
        }

        public void FireWeapon(int idx)
        {
            if(!GameStageManager.Instance.IsGameStarted || idx < 0 || idx >= Weapons.Count) {
                return;
            }
            Weapons.ElementAt(idx).Fire(_fighter);
        }

        public void FireAllWeapons()
        {
            for(int i=0; i<_weapons.Count; ++i) {
                FireWeapon(i);
            }
        }

        public void UseSpecial(SpecialData.SpecialType type)
        {
            if(!GameStageManager.Instance.IsGameStarted) {
                return;
            }

            Special special = _specials.GetOrDefault(type);
            special?.Use();
        }

        public int GetSpecialRemaining(SpecialData.SpecialType type)
        {
            Special special = _specials.GetOrDefault(type);
            return special?.RemainingUses ?? 0;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Fighter Stats:");
            builder.AppendLine($"Health: {CurrentHealth} / {DataManager.Instance.GameData.Fighter.MaxHealth} ({IsDead})");
            builder.AppendLine(Brain?.ToString());
            builder.AppendLine(Armor.ToString());
            builder.AppendLine("Weapons:");
            foreach(Weapon weapon in _weapons) {
                builder.AppendLine(weapon.ToString());
            }
            builder.AppendLine("Specials:");
            foreach(var kvp in Specials) {
                builder.AppendLine($"{kvp.Key}: {kvp.Value}");
            }
            builder.AppendLine($"Move Speed: {MoveSpeed}");
            return builder.ToString();
        }
    }
}
