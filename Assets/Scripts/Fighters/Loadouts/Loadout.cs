using System.Collections.Generic;

using CatFight.Players;
using CatFight.Players.Schematics;

using UnityEngine;

namespace CatFight.Fighters.Loadouts
{
    public sealed class Loadout
    {
        private readonly Dictionary<int, LoadoutSlot> _slots = new Dictionary<int, LoadoutSlot>();

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
                    LoadoutSlot loadoutSlot;
                    if(!_slots.TryGetValue(kvp.Key, out loadoutSlot)) {
                        loadoutSlot = LoadoutSlotFactory.Create(kvp.Value.SlotData);
                        if(null == loadoutSlot) {
                            continue;
                        }
                        _slots[kvp.Key] = loadoutSlot;
                    }
                    loadoutSlot.Process(kvp.Value);
                }
            }

            foreach(var kvp in _slots) {
                kvp.Value.Complete();
            }
        }
    }
}
