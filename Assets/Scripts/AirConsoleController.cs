using System;

using NDream.AirConsole;
using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

using CatFight.Util;

namespace CatFight
{
    [RequireComponent(typeof(AirConsole))]
    public sealed class AirConsoleController : SingletonBehavior<AirConsoleController>
    {
#region Events
        public event EventHandler<MessageEvent> MessageEvent;
#endregion

#region Unity Lifecycle
        private void Start()
        {
            AirConsole.instance.onConnect += OnConnect;
            AirConsole.instance.onDisconnect += OnDisconnect;
            AirConsole.instance.onMessage += OnMessage;

            SceneManager.LoadSceneAsync("lobby", LoadSceneMode.Additive);
        }

        protected override void OnDestroy()
        {
            AirConsole.instance.onMessage -= OnMessage;
            AirConsole.instance.onDisconnect -= OnDisconnect;
            AirConsole.instance.onConnect -= OnConnect;
        }
#endregion

#region Event Handlers
        private void OnConnect(int deviceId)
        {
            Debug.Log($"OnConnect({deviceId})");

            PlayerManager.Instance.ConnectPlayer(deviceId);
        }

        private void OnDisconnect(int deviceId)
        {
            Debug.Log($"OnDisconnect({deviceId})");

            PlayerManager.Instance.DisconnectPlayer(deviceId);
        }

        private void OnMessage(int from, JToken data)
        {
            Debug.Log($"OnMessage({from}: {data})");

            MessageEvent?.Invoke(this, new MessageEvent(from, data));
        }
#endregion
    }
}
