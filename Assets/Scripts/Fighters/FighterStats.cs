using System;
using System.Collections.Generic;

using CatFight.Data;
using CatFight.Fighters.Loadouts;
using CatFight.Items.Specials;
using CatFight.Items.Weapons;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters
{
    [Serializable]
    public sealed class FighterStats
    {
        [SerializeField]
        [ReadOnly]
        private float _currentHealth = 100.0f;

        public float CurrentHealth { get { return _currentHealth; } private set { _currentHealth = value < 0.0f ? 0.0f : value; } }

        public bool IsDead => CurrentHealth <= 0.0f;

// TODO: armor

// TODO: debug serialize fields

        private readonly List<Weapon> _weapons = new List<Weapon>();

        private readonly Dictionary<string, Special> _specials = new Dictionary<string, Special>();

        public float MoveSpeed => _fighterData.MoveSpeed;

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
                    BrainLoadoutSlot brainSlot = (BrainLoadoutSlot)slot;
                    if(null != brainSlot.Brain) {
                        fsm.SetFsmTemplate(brainSlot.Brain.LoadBrain());
                    }
                    break;
                case SchematicSlotData.SchematicSlotTypeWeapon:
                    WeaponLoadoutSlot weaponSlot = (WeaponLoadoutSlot)slot;
                    if(null != weaponSlot.Weapon) {
                        _weapons.Add(weaponSlot.Weapon);
                    }
                    break;
                case SchematicSlotData.SchematicSlotTypeArmor:
                    ArmorLoadoutSlot armorSlot = (ArmorLoadoutSlot)slot;
// TODO
                    break;
                case SchematicSlotData.SchematicSlotTypeSpecial:
                    SpecialLoadoutSlot specialSlot = (SpecialLoadoutSlot)slot;
                    foreach(var specialKvp in specialSlot.Specials) {
                        Special special = _specials.GetOrDefault(specialKvp.Key);
                        if(null == special) {
                            special = SpecialFactory.Create(specialKvp.Key, specialKvp.Value);
                            _specials.Add(specialKvp.Key, special);
                        } else {
                            special.IncreaseTotalUses(specialKvp.Value);
                        }
                    }
                    break;
                }
            }
        }

        public void Damage(float amount)
        {
            //float reducedAmount = amount - (amount * ArmorReduction);
            //CurrentHealth -= reducedAmount;
        }

        public void FireWeapon(int idx)
        {
            if(idx < 0 || idx >= _weapons.Count) {
                return;
            }
            _weapons[idx].Fire();
        }

        public void UseSpecial(string type)
        {
            Special special = _specials.GetOrDefault(type);
            special?.Use();
        }
    }
}
