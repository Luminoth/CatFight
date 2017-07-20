using CatFight.AirConsole;
using CatFight.AirConsole.Messages;

using UnityEngine;

namespace CatFight.Scenes
{
    public sealed class Staging : Stage<Staging>
    {
        private void SetSlot(int deviceId, int slotId, int itemId)
        {
            Player player;
            if(!PlayerManager.Instance.Players.TryGetValue(deviceId, out player)) {
                Debug.LogError($"Cannot set slot {slotId} to {itemId} for non-existant player {deviceId}");
                return;
            }

            player.Schematic.SetSlot(slotId, itemId);
        }

        private void ClearSlot(int deviceId, int slotId)
        {
            Player player;
            if(!PlayerManager.Instance.Players.TryGetValue(deviceId, out player)) {
                Debug.LogError($"Cannot clear slot {slotId} for non-existant player {deviceId}");
                return;
            }

            player.Schematic.ClearSlot(slotId);
        }

#region Event Handlers
        protected override void MessageEventHandler(object sender, MessageEventArgs evt)
        {
            switch(evt.Message.type)
            {
            case Message.MessageType.SetSlot:
                SetSlotMessage setSlotMessage = (SetSlotMessage)evt.Message;
                SetSlot(evt.From, setSlotMessage.slotId, setSlotMessage.itemId);
                break;
            case Message.MessageType.ClearSlot:
                ClearSlotMessage clearSlotMessage = (ClearSlotMessage)evt.Message;
                ClearSlot(evt.From, clearSlotMessage.slotId);
                break;
            case Message.MessageType.ConfirmStaging:
                ConfirmStagingMessage confirmStagingMessage = (ConfirmStagingMessage)evt.Message;
                PlayerManager.Instance.ConfirmPlayerSchematic(evt.From, confirmStagingMessage.isConfirmed);

                if(PlayerManager.Instance.AreAllPlayersReady()) {
                    GameStageManager.Instance.LoadArena();
                }
                break;
            default:
                Debug.LogWarning($"Ignoring unexpected message type {evt.Message.type} from {evt.From}");
                break;
            }
        }
#endregion
    }
}
