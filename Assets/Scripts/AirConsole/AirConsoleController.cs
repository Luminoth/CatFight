using System;

using CatFight.AirConsole.Messages;
using CatFight.Data;
using CatFight.Util;

using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatFight.AirConsole
{
    [RequireComponent(typeof(NDream.AirConsole.AirConsole))]
    public sealed class AirConsoleController : SingletonBehavior<AirConsoleController>
    {
#region Events
        public event EventHandler<MessageEvent> MessageEvent;
#endregion

        [SerializeField]
        private TextAsset _gameDataFile;

        public TextAsset GameDataFile => _gameDataFile;

        public GameData GameData { get; private set; }

#region Unity Lifecycle
        private void Start()
        {
            if(!LoadGameData()) {
                return;
            }

            NDream.AirConsole.AirConsole.instance.onConnect += OnConnect;
            NDream.AirConsole.AirConsole.instance.onDisconnect += OnDisconnect;
            NDream.AirConsole.AirConsole.instance.onMessage += OnMessage;

            SceneManager.LoadSceneAsync("lobby", LoadSceneMode.Additive);
        }

        protected override void OnDestroy()
        {
            if(null != NDream.AirConsole.AirConsole.instance) {
                NDream.AirConsole.AirConsole.instance.onMessage -= OnMessage;
                NDream.AirConsole.AirConsole.instance.onDisconnect -= OnDisconnect;
                NDream.AirConsole.AirConsole.instance.onConnect -= OnConnect;
            }
        }
#endregion

        private bool LoadGameData()
        {
            Debug.Log("Loading game data...");
            //Debug.Log(GameDataFile.text);

// TODO: show an error dialog
            GameData = JsonUtility.FromJson<GameData>(GameDataFile.text);
            if(null == GameData) {
                Debug.LogError("There was an error loading the game data!");
                return false;
            }

            GameData.Process();
            GameData.DebugDump();

            return true;
        }

        public void Message(int to, Message message)
        {
            NDream.AirConsole.AirConsole.instance.Message(to, message);
        }

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
