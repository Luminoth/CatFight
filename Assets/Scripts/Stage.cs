using CatFight.AirConsole;
using CatFight.Util;

namespace CatFight
{
    public abstract class Stage<T> : SingletonBehavior<T> where T: SingletonBehavior<T>
    {
        protected virtual void Start()
        {
            AirConsoleManager.Instance.MessageEvent += MessageEventHandler;
        }

        protected override void OnDestroy()
        {
            if(AirConsoleManager.HasInstance) {
                AirConsoleManager.Instance.MessageEvent -= MessageEventHandler;
            }

            base.OnDestroy();
        }

        protected abstract void MessageEventHandler(object sender, MessageEventArgs evt);
    }
}
