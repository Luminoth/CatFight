using CatFight.Data;

using UnityEngine;

namespace CatFight.Items
{
    public static class ItemFactory
    {
        public static Item Create(ItemData data)
        {
            switch(data.ItemType)
            {
            case Item.ItemType.Brain:
                return new BrainItem(data);
            case Item.ItemType.Armor:
                return new ArmorItem(data);
            case Item.ItemType.Special:
                return new SpecialItem(data);
            case Item.ItemType.Weapon:
                return new WeaponItem(data);
            default:
                Debug.LogError($"Invalid item type {data.ItemType}!");
                return null;
            }
        }
    }

    public abstract class Item
    {
        public enum ItemType
        {
            Brain,
            Armor,
            Weapon,
            Special,
        }

        public abstract ItemType Type { get; }

        protected Item(ItemData data)
        {
        }
    }
}
