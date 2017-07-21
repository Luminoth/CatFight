using System;

using CatFight.Data;
using CatFight.Players.Schematics;

namespace CatFight.Fighters.Loadouts
{
    [Serializable]
    public sealed class WeaponLoadoutSlot : LoadoutSlot
    {
        public WeaponLoadoutSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }

        public override void Process(SchematicSlot schematicSlot)
        {
            WeaponSchematicSlot weaponSlot = (WeaponSchematicSlot)schematicSlot;
            if(null == weaponSlot.Item) {
                return;
            }
        }

        public override void Complete()
        {
        }
    }
}
