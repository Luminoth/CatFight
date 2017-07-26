using System;
using System.Collections.Generic;

using CatFight.Data;
using CatFight.Fighters.Loadouts;
using CatFight.Items.Armor;
using CatFight.Items.Specials;
using CatFight.Items.Weapons;
using CatFight.Util;

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

#region Armor
        [SerializeField]
        [ReadOnly]
        private readonly Armor _armor = new Armor();
#endregion

#region Weapons
        private readonly List<Weapon> _weapons = new List<Weapon>();

        public int WeaponCount => _weapons.Count;
#endregion

#region Specials
        private readonly Dictionary<int, Special> _specials = new Dictionary<int, Special>();
#endregion

#region Movement
        public float MoveSpeed => _fighterData.MoveSpeed;
#endregion

        private readonly FighterData _fighterData;

        private readonly Fighter _fighter;

        public FighterStats(Fighter fighter, FighterData fighterData)
        {
            _fighter = fighter;
            _fighterData = fighterData;

            CurrentHealth = _fighterData.MaxHealth;
        }

        public void Compile(Loadout loadout, PlayMakerFSM fsm)
        {
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
                fsm.SetFsmTemplate(brainSlot.Brain.LoadBrain());
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
    }
}
