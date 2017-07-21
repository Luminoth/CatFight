using CatFight.Data;

namespace CatFight.Items
{
    public sealed class BrainItem : Item
    {
        public override ItemType Type => ItemType.Brain;

        public BrainItem(ItemData data)
            : base(data)
        {
        }
    }
}
