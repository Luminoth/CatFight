using CatFight.Data;

namespace CatFight.Items
{
    public sealed class WeaponItem : Item
    {
        public override ItemType Type => ItemType.Weapon;

        public WeaponItem(ItemData data)
            : base(data)
        {
        }
    }
}
