using CatFight.Data;

namespace CatFight.Items
{
    public sealed class ArmorItem : Item
    {
        public override ItemType Type => ItemType.Armor;

        public ArmorItem(ItemData data)
            : base(data)
        {
        }
    }
}
