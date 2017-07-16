using NDream.AirConsole;

using CatFight.Util;

namespace CatFight
{
    public sealed class Lobby : SingletonBehavior<Lobby>
    {
        private void Start()
        {
            AirConsoleController.Instance.MessageEvent += MessageEventHandler;
        }

        protected override void OnDestroy()
        {
            AirConsoleController.Instance.MessageEvent -= MessageEventHandler;
        }

        private void MessageEventHandler(object sender, MessageEvent evt)
        {
            AirConsole.instance.Message(evt.From, "Hello World!");
        }
    }
}
