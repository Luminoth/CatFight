using System.Collections.Generic;

using CatFight.AirConsole.Messages;
using CatFight.Data;

using UnityEngine;

namespace CatFight.Players.Schematics
{
    public sealed class Schematic
    {
        public SchematicData SchematicData { get; }

        private readonly Dictionary<int, SchematicSlot> _slots = new Dictionary<int, SchematicSlot>();

        public bool IsConfirmed { get; set; }

        private int _filledSlotCount;

        private readonly Player _player;

        public Schematic(Player player, SchematicData data)
        {
            _player = player;
            SchematicData = data;

            foreach(SchematicSlotData slotData in SchematicData.Slots) {
                //Debug.Log($"Adding schematic slot {slotData.Id}: {slotData.Name} - {slotData.Type}");
                _slots.Add(slotData.Id, SchematicSlotFactory.Create(slotData));
            }
        }

        public bool SetSlot(int slotId, int itemId)
        {
            if(_filledSlotCount >= SchematicData.MaxFilledSlots) {
                return false;
            }

// TODO: error check (also validate the itemId is legit)
            _slots[slotId].ItemId = itemId;
            ++_filledSlotCount;

            PlayerManager.Instance.BroadcastToTeam(_player.Team.Id, new SetSlotMessage
                {
                    slotId = slotId,
                    itemId = itemId
                },
                _player.DeviceId
            );

            return true;

        }

        public void ClearSlot(int slotId)
        {
// TODO: error check
            _slots[slotId].ItemId = 0;
            --_filledSlotCount;

            PlayerManager.Instance.BroadcastToTeam(_player.Team.Id, new ClearSlotMessage
                {
                    slotId = slotId
                },
                _player.DeviceId
            );
        }
    }
}
