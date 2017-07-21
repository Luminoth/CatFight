using CatFight.Players.Schematics;

namespace CatFight.Fighters
{
    public static class LoadoutSlotFactory
    {
        public static LoadoutSlot Create(SchematicSlot schematicSlot)
        {
            switch(schematicSlot.SlotData.Type)
            {
            default:
                return null;
            }
        }
    }

    public abstract class LoadoutSlot
    {
    }
}
