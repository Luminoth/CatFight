using CatFight.AirConsole;
using CatFight.AirConsole.Messages;
using CatFight.Players;

using UnityEngine;

namespace CatFight.Stages.Staging
{
    public sealed class Staging : Stage
    {
        public new static Staging Instance => (Staging)Stage.Instance;

        private void SetSlot(int deviceId, int slotId, int itemId)
        {
            Player player = PlayerManager.Instance.GetPlayer(deviceId);
            Debug.Log($"Player {player?.DeviceId} setting slot {slotId} to {itemId}");
            player?.Schematic.SetSlot(slotId, itemId);
        }

        private void ClearSlot(int deviceId, int slotId)
        {
            Player player = PlayerManager.Instance.GetPlayer(deviceId);
            Debug.Log($"Player {player?.DeviceId} clearing slot {slotId}");
            player?.Schematic.ClearSlot(slotId);
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
