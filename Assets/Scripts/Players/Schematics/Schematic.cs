using System;
using System.Collections.Generic;
using System.Linq;

using CatFight.AirConsole.Messages;
using CatFight.Data;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Players.Schematics
{
    [Serializable]
    public sealed class Schematic
    {
        public SchematicData SchematicData { get; }

        private readonly Dictionary<int, SchematicSlot> _slots = new Dictionary<int, SchematicSlot>();

        public IReadOnlyDictionary<int, SchematicSlot> Slots => _slots;

#if UNITY_EDITOR
        [SerializeField]
        [ReadOnly]
        private SchematicSlot[] _debugSlots;
#endif

        [SerializeField]
        [ReadOnly]
        private bool _isConfirmed;

        public bool IsConfirmed { get { return _isConfirmed; } set { _isConfirmed = value; } }

        [SerializeField]
        [ReadOnly]
        private int _filledSlotCount;

        private readonly Player _player;

        public Schematic(Player player, SchematicData data)
        {
            _player = player;
            SchematicData = data;

            foreach(SchematicSlotData slotData in SchematicData.Slots) {
                SchematicSlot slot = SchematicSlotFactory.Create(slotData);
                if(null == slot) {
                    continue;
                }
                _slots.Add(slotData.Id, slot);
            }

#if UNITY_EDITOR
            _debugSlots = _slots.Values.ToArray();
#endif
        }

        public bool SetSlot(int slotId, int itemId)
        {
            if(_filledSlotCount >= SchematicData.MaxFilledSlots) {
                Debug.LogWarning($"Player {_player.DeviceId} has max slots filled!");
                return false;
            }

// TODO: error check (also validate the itemId is legit)
            _slots[slotId].ItemId = itemId;
            ++_filledSlotCount;

            PlayerManager.Instance.BroadcastToTeam(_player.TeamId, new SetSlotMessage
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
            int itemId = _slots[slotId].ItemId;
            _slots[slotId].ItemId = 0;
            --_filledSlotCount;

            PlayerManager.Instance.BroadcastToTeam(_player.TeamId, new ClearSlotMessage
                {
                    slotId = slotId,
                    itemId = itemId
                },
                _player.DeviceId
            );
        }
    }
}
