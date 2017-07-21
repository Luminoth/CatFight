using CatFight.Data;
using CatFight.Players.Schematics;

namespace CatFight.Fighters.Loadouts
{
    public sealed class ArmorLoadoutSlot : LoadoutSlot
    {
        public float ArmorReduction { get; private set; }

        public float MoveModifier { get; private set; } = 1.0f;

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
            ArmorReduction += (1.0f - ArmorReduction) * armorSlot.ArmorItem.ArmorReduction;
            MoveModifier += MoveModifier * armorSlot.ArmorItem.MoveModifier;
        }

        public override void Complete()
        {
        }
    }
}
