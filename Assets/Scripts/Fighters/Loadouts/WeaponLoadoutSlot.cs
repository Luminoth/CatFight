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
        private readonly Dictionary<string, int> _weaponTypeVotes = new Dictionary<string, int>();

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

            int currentCount = _weaponTypeVotes.GetOrDefault(weaponSlot.WeaponItem.Type);
            _weaponTypeVotes[weaponSlot.WeaponItem.Type] = currentCount + 1;
        }

        public override void Complete()
        {
            string winnerType = VoteHelper.GetWinner(_weaponTypeVotes);
            if(string.IsNullOrWhiteSpace(winnerType)) {
                return;
            }

            Weapon = WeaponFactory.Create(winnerType);
            Weapon.SetStrength(_weaponTypeVotes[winnerType]);
        }
    }
}
