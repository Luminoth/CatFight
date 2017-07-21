using CatFight.AirConsole;
using CatFight.Util;

namespace CatFight.Stages
{
    public abstract class Stage : MonoBehavior
    {
        public static Stage Instance { get; private set; }

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Instance = this;
        }

        protected virtual void Start()
        {
            AirConsoleManager.Instance.MessageEvent += MessageEventHandler;
        }

        protected virtual void OnDestroy()
        {
            if(AirConsoleManager.HasInstance) {
                AirConsoleManager.Instance.MessageEvent -= MessageEventHandler;
            }

            Instance = null;
        }
#endregion

#region Event Handlers
        protected abstract void MessageEventHandler(object sender, MessageEventArgs evt);
#endregion
    }
}
