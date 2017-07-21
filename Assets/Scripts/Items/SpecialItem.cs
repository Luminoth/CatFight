using CatFight.Data;

namespace CatFight.Items
{
    public sealed class SpecialItem : Item
    {
        public override ItemType Type => ItemType.Special;

        public SpecialItem(ItemData data)
            : base(data)
        {
        }
    }
}
