using System;
using System.Collections.Generic;

using CatFight.Data;
using CatFight.Items.Weapons;
using CatFight.Players.Schematics;
using CatFight.Util;

using JetBrains.Annotations;

namespace CatFight.Fighters.Loadouts
{
    [Serializable]
    public sealed class WeaponLoadoutSlot : LoadoutSlot
    {
        private readonly Dictionary<int, int> _weaponTypeVotes = new Dictionary<int, int>();

        [CanBeNull]
        public Weapon Weapon { get; private set; }

        public WeaponLoadoutSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }

        public override void Process(SchematicSlot schematicSlot)
        {
            WeaponSchematicSlot weaponSlot = (WeaponSchematicSlot)schematicSlot;
            if(null == weaponSlot.WeaponItem) {
                return;
            }

            int currentCount = _weaponTypeVotes.GetOrDefault(weaponSlot.WeaponItem.Id);
            _weaponTypeVotes[weaponSlot.WeaponItem.Id] = currentCount + 1;
        }

        public override void Complete()
        {
            int winnerType = VoteHelper.GetWinner(_weaponTypeVotes);
            WeaponData.WeaponDataEntry weaponData = DataManager.Instance.GameData.Weapons.Entries.GetOrDefault(winnerType);
            if(null == weaponData) {
                return;
            }

            Weapon = WeaponFactory.Create(weaponData);
            if(null != Weapon) {
                Weapon.Strength = _weaponTypeVotes[winnerType];
            }
        }
    }
}
