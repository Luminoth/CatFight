using CatFight.Data;
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

        public void Initialize(Loadout loadout)
        {
            SchematicSlotData slotData = DataManager.Instance.GameData.Fighter.Schematic.Entries.GetOrDefault(_slotId);
            if(null == slotData?.SlotPrefab) {
                return;
            }

            Instantiate(slotData.SlotPrefab, transform);
        }
    }
}
