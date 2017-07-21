using System;
using System.Collections.Generic;
using System.Linq;

using CatFight.Players;
using CatFight.Players.Schematics;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters.Loadouts
{
    [Serializable]
    public sealed class Loadout
    {
        private readonly Dictionary<int, LoadoutSlot> _slots = new Dictionary<int, LoadoutSlot>();

        public IReadOnlyDictionary<int, LoadoutSlot> Slots => _slots;

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

        public void BuildLoadout()
        {
            Debug.Log($"Building loadout for team {_fighter.TeamId}'s fighter...");

            _slots.Clear();

            var team = PlayerManager.Instance.GetTeam(_fighter.TeamId);
            foreach(Player player in team) {
                Schematic schematic = player.Schematic;
                foreach(var kvp in schematic.Slots) {
                    LoadoutSlot loadoutSlot = _slots.GetOrDefault(kvp.Key);
                    if(null == loadoutSlot) {
                        loadoutSlot = LoadoutSlotFactory.Create(kvp.Value.SlotData);
                        if(null == loadoutSlot) {
                            continue;
                        }
                        _slots[kvp.Key] = loadoutSlot;
                    }
                    loadoutSlot.Process(kvp.Value);
                }
            }

#if UNITY_EDITOR
            _debugSlots = _slots.Values.ToArray();
#endif

            foreach(var kvp in _slots) {
                kvp.Value.Complete();
            }
        }
    }
}
