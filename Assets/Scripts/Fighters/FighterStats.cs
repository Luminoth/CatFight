using System;
using System.Collections.Generic;
using System.Text;

using CatFight.Data;
using CatFight.Fighters.Loadouts;
using CatFight.Items.Armor;
using CatFight.Items.Brains;
using CatFight.Items.Specials;
using CatFight.Items.Weapons;
using CatFight.Util;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Fighters
{
    [Serializable]
    public sealed class FighterStats
    {
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

        public int WeaponCount => _weapons.Count;
#endregion

#region Specials
        private readonly Dictionary<int, Special> _specials = new Dictionary<int, Special>();

        public IReadOnlyDictionary<int, Special> Specials => _specials;
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

            foreach(var kvp in loadout.Slots) {
                LoadoutSlot slot = kvp.Value;
                switch(slot.SlotData.Type)
                {
                case SchematicSlotData.SchematicSlotTypeBrain:
                    SetBrain((BrainLoadoutSlot)slot, fsm);
                    break;
                case SchematicSlotData.SchematicSlotTypeWeapon:
                    AddWeapon((WeaponLoadoutSlot)slot);
                    break;
                case SchematicSlotData.SchematicSlotTypeArmor:
                    IncreaseArmorStrength((ArmorLoadoutSlot)slot);
                    break;
                case SchematicSlotData.SchematicSlotTypeSpecial:
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
                fsm.SetFsmTemplate(Brain?.LoadBrain());
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
                ArmorData armorData = DataManager.Instance.GameData.GetItem(ItemData.ItemTypeArmor, kvp.Key) as ArmorData;
                if(null == armorData) {
                    continue;
                }

                _armor.IncreaseStrength(armorData.Type, kvp.Value, armorData.ReductionPercent, DataManager.Instance.GameData.Fighter.ArmorReductionCapPercent);
            }
        }

        private void IncreaseSpecialUses(SpecialLoadoutSlot specialSlot)
        {
            foreach(var kvp in specialSlot.Specials) {
                Special special = _specials.GetOrDefault(kvp.Key);
                if(null == special) {
                    SpecialData specialData = DataManager.Instance.GameData.GetItem(ItemData.ItemTypeSpecial, kvp.Key) as SpecialData;
                    if(null == specialData) {
                        continue;
                    }

                    special = SpecialFactory.Create(specialData.Type, kvp.Value);
                    _specials.Add(kvp.Key, special);
                } else {
                    special.IncreaseTotalUses(kvp.Value);
                }
            }
        }
#endregion

        public void Damage(int amount, string type)
        {
            float reducedAmount = amount - (amount * _armor.GetDamageReduction(type));
            CurrentHealth -= reducedAmount;
        }

        public void FireWeapon(int idx)
        {
            if(idx < 0 || idx >= _weapons.Count) {
                return;
            }
            _weapons[idx].Fire();
        }

        public void UseSpecial(int id)
        {
            Special special = _specials.GetOrDefault(id);
            special?.Use();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Fighter Stats:");
            builder.AppendLine($"Health: {CurrentHealth} / {DataManager.Instance.GameData.Fighter.MaxHealth} ({IsDead})");
            builder.AppendLine(Brain?.ToString());
            builder.AppendLine(Armor.ToString());
            builder.AppendLine($"Weapons ({WeaponCount}):");
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
