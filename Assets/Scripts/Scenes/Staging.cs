using CatFight.AirConsole;
using CatFight.AirConsole.Messages;
using CatFight.Util;

namespace CatFight.Scenes
{
    public sealed class Staging : SingletonBehavior<Staging>
    {
#region Unity Lifecycle
        private void Start()
        {
            AirConsoleManager.Instance.MessageEvent += MessageEventHandler;
        }

        protected override void OnDestroy()
        {
            AirConsoleManager.Instance.MessageEvent -= MessageEventHandler;
        }
#endregion

#region Event Handlers
        private void MessageEventHandler(object sender, MessageEventArgs evt)
        {
            switch(evt.Message.type)
            {
            case Message.MessageType.ConfirmStaging:
                ConfirmStagingMessage message = (ConfirmStagingMessage)evt.Message;
                PlayerManager.Instance.ConfirmPlayerSchematic(evt.From, message.isConfirmed);

                if(PlayerManager.Instance.AreAllPlayersReady()) {
                    GameStageManager.Instance.LoadArena();
                }
                break;
            }
        }
#endregion
    }
}
