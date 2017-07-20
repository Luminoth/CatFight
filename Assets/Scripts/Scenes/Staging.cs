using CatFight.AirConsole;
using CatFight.AirConsole.Messages;

using UnityEngine;

namespace CatFight.Scenes
{
    public sealed class Staging : Stage<Staging>
    {
        {
        }

        {
            }
        }

#region Event Handlers
        protected override void MessageEventHandler(object sender, MessageEventArgs evt)
        {
            switch(evt.Message.type)
            {
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
