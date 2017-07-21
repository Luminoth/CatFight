using System;

using CatFight.Data;
using CatFight.Players.Schematics;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters.Loadouts
{
    [Serializable]
    public sealed class ArmorLoadoutSlot : LoadoutSlot
    {
        [SerializeField]
        [ReadOnly]
        private float _armorReduction;

        public float ArmorReduction { get { return _armorReduction; } private set { _armorReduction = value; } }

        [SerializeField]
        [ReadOnly]
        private float _moveModifier = 1.0f;

        public float MoveModifier { get { return _moveModifier; } private set { _moveModifier = value; } }

        public ArmorLoadoutSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }

        public float CalculateNewArmorReduction(float currentArmorReduction)
        {
            return currentArmorReduction + (1.0f - currentArmorReduction) * ArmorReduction;
        }

        public float CalculateNewMoveModifier(float currentMoveModifier)
        {
            return currentMoveModifier * MoveModifier;
        }

        public override void Process(SchematicSlot schematicSlot)
        {
            ArmorSchematicSlot armorSlot = (ArmorSchematicSlot)schematicSlot;
            if(null == armorSlot.Item) {
                return;
            }

            ArmorReduction += (1.0f - ArmorReduction) * armorSlot.ArmorItem.ArmorReduction;
            MoveModifier += MoveModifier * armorSlot.ArmorItem.MoveModifier;
        }

        public override void Complete()
        {
        }
    }
}
