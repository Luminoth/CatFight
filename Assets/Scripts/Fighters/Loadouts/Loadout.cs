using System;
using System.Collections.Generic;
using System.Text;

using CatFight.Players;
using CatFight.Players.Schematics;
using CatFight.Util;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Fighters.Loadouts
{
    [Serializable]
    public sealed class Loadout
    {
        private readonly Dictionary<int, LoadoutSlot> _slots = new Dictionary<int, LoadoutSlot>();

        private readonly List<LoadoutSlot> _slotList = new List<LoadoutSlot>();

        public IReadOnlyCollection<LoadoutSlot> Slots => _slotList;

#if UNITY_EDITOR
        [SerializeField]
        [ReadOnly]
        private LoadoutSlot[] _debugSlots;
#endif

        private readonly Fighter _fighter;

        public Loadout(Fighter fighter)
        {
            _fighter = fighter;
        }

        public void Initialize()
        {
            Debug.Log($"Building loadout for team {_fighter.Team.Id}'s fighter...");

            _slots.Clear();
            _slotList.Clear();

            Team team = PlayerManager.Instance.GetTeam(_fighter.Team.Id);
            foreach(Player player in team.Players) {
                Schematic schematic = player.Schematic;
                foreach(SchematicSlot slot in schematic.Slots) {
                    LoadoutSlot loadoutSlot = _slots.GetOrDefault(slot.SlotData.Id);
                    if(null == loadoutSlot) {
                        loadoutSlot = LoadoutSlotFactory.Create(_fighter, slot.SlotData);
                        if(null == loadoutSlot) {
                            continue;
                        }

                        _slots.Add(slot.SlotData.Id, loadoutSlot);
                        _slotList.Add(loadoutSlot);
                    }
                    loadoutSlot.Process(slot);
                }
            }

#if UNITY_EDITOR
            _debugSlots = _slotList.ToArray();
#endif

            foreach(LoadoutSlot slot in Slots) {
                slot.Complete();
            }
        }

        [CanBeNull]
        public LoadoutSlot GetSlot(int slotId)
        {
            return _slots.GetOrDefault(slotId);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Loadout:");
            foreach(LoadoutSlot slot in Slots) {
                builder.AppendLine($"Slot {slot.SlotData.Id}: {slot}");
            }
            return builder.ToString();
        }
    }
}
