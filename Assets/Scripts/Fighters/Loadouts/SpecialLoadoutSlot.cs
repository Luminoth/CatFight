using CatFight.Data;
using CatFight.Players.Schematics;

namespace CatFight.Fighters.Loadouts
{
    public sealed class SpecialLoadoutSlot : LoadoutSlot
    {
        public SpecialLoadoutSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }

        public override void Process(SchematicSlot schematicSlot)
        {
        }

        public override void Complete()
        {
        }
    }
}
