using System;
using System.Collections.Generic;

using CatFight.Data;
using CatFight.Fighters.Loadouts;
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

/*
        [SerializeField]
        [ReadOnly]
        private float _armorReduction;

        public float ArmorReduction { get { return _armorReduction; } private set { _armorReduction = Mathf.Clamp01(value); } }

        [SerializeField]
        [ReadOnly]
        private float _moveModifier = 1.0f;

        public float MoveModifier { get { return _moveModifier; } private set { _moveModifier = value < 0.0f ? 0.0f : value; } }
*/

        public bool IsDead => CurrentHealth <= 0.0f;

        private readonly List<Weapon> _weapons = new List<Weapon>();

        private readonly FighterData _fighterData;

        private readonly Fighter _fighter;

        public float MoveSpeed => _fighterData.MoveSpeed /** MoveModifier*/;

        public FighterStats(Fighter fighter, FighterData fighterData)
        {
            _fighter = fighter;
            _fighterData = fighterData;

            CurrentHealth = _fighterData.MaxHealth;
        }

        public void Compile(Loadout loadout)
        {
            foreach(var kvp in loadout.Slots) {
                LoadoutSlot slot = kvp.Value;
                switch(slot.SlotData.Type)
                {
                case SchematicSlotData.SchematicSlotTypeBrain:
                    break;
                case SchematicSlotData.SchematicSlotTypeWeapon:
                    WeaponLoadoutSlot weaponSlot = (WeaponLoadoutSlot)slot;
                    _weapons.Add(weaponSlot.Weapon);
                    break;
                case SchematicSlotData.SchematicSlotTypeArmor:
                    ArmorLoadoutSlot armorSlot = (ArmorLoadoutSlot)slot;
                    /*ArmorReduction = armorSlot.CalculateNewArmorReduction(ArmorReduction);
                    MoveModifier = armorSlot.CalculateNewMoveModifier(MoveModifier);*/
                    break;
                case SchematicSlotData.SchematicSlotTypeSpecial:
                    break;
                }
            }
        }

        public void Damage(float amount)
        {
            //float reducedAmount = amount - (amount * ArmorReduction);
            //CurrentHealth -= reducedAmount;
        }
    }
}
