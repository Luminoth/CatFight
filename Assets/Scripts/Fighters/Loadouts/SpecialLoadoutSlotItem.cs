using CatFight.Data;

using UnityEngine;

namespace CatFight.Fighters.Loadouts
{
    public sealed class SpecialLoadoutSlotItem : LoadoutSlotItem
    {
        [SerializeField]
        private SpecialData.SpecialType _specialType;

        public SpecialData.SpecialType Type => _specialType;

        [SerializeField]
        private Transform _ammoTransform;

        public Transform AmmoTransform => _ammoTransform;
    }
}
