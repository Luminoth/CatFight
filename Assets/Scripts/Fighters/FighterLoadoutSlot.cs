using CatFight.Fighters.Loadouts;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters
{
    public sealed class FighterLoadoutSlot : MonoBehavior
    {
        [SerializeField]
        private int _slotId;

        public int SlotId => _slotId;

        private LoadoutSlot _slot;

        public LoadoutSlotItem SlotItem { get; private set; }

        public void Initialize(Loadout loadout)
        {
            _slot = loadout.GetSlot(SlotId);

            LoadoutSlotItem slotItemPrefab = _slot?.GetSlotItemPrefab();
            SlotItem = null == slotItemPrefab ? null : Instantiate(slotItemPrefab, transform);
        }
    }
}
