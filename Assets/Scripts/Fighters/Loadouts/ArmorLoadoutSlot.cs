using System;
using System.Collections.Generic;

using CatFight.Data;
using CatFight.Items.Armor;
using CatFight.Players.Schematics;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters.Loadouts
{
    [Serializable]
    public sealed class ArmorLoadoutSlot : LoadoutSlot
    {
        private readonly Dictionary<string, int> _armorTypeVotes = new Dictionary<string, int>();

/*
        [SerializeField]
        [ReadOnly]
        private float _armorReduction;

        public float ArmorReduction { get { return _armorReduction; } private set { _armorReduction = value; } }

        [SerializeField]
        [ReadOnly]
        private float _moveModifier = 1.0f;

        public float MoveModifier { get { return _moveModifier; } private set { _moveModifier = value; } }
*/

        public ArmorLoadoutSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }

/*
        public float CalculateNewArmorReduction(float currentArmorReduction)
        {
            return currentArmorReduction + (1.0f - currentArmorReduction) * ArmorReduction;
        }

        public float CalculateNewMoveModifier(float currentMoveModifier)
        {
            return currentMoveModifier * MoveModifier;
        }
*/

        public override void Process(SchematicSlot schematicSlot)
        {
            ArmorSchematicSlot armorSlot = (ArmorSchematicSlot)schematicSlot;
            if(null == armorSlot.Item || null == armorSlot.ArmorItem) {
                return;
            }

/*
            ArmorReduction += (1.0f - ArmorReduction) * armorSlot.ArmorItem.ArmorReduction;
            MoveModifier += MoveModifier * armorSlot.ArmorItem.MoveModifier;
*/

            int currentCount = _armorTypeVotes.GetOrDefault(armorSlot.ArmorItem.Type);
            _armorTypeVotes[armorSlot.ArmorItem.Type] = currentCount + 1;
        }

        public override void Complete()
        {
// TODO: armor is a combination of anti-stuff at vote-strength
        }
    }
}
