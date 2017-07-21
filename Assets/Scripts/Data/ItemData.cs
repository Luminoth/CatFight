using CatFight.Items;

namespace CatFight.Data
{
    public abstract class ItemData : Data
    {
        public abstract Item.ItemType ItemType { get; }
    }
}
