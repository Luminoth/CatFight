using System;

using CatFight.Data;
using CatFight.Util;

using NDream.AirConsole;
using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatFight
{
    [RequireComponent(typeof(AirConsole))]
    public sealed class AirConsoleController : SingletonBehavior<AirConsoleController>
    {
#region Events
        public event EventHandler<MessageEvent> MessageEvent;
#endregion

        [SerializeField]
        private TextAsset _gameDataFile;

        public TextAsset GameDataFile => _gameDataFile;

        private GameData _gameData;

#region Unity Lifecycle
        private void Start()
        {
            if(!LoadGameData()) {
                return;
            }

            AirConsole.instance.onConnect += OnConnect;
            AirConsole.instance.onDisconnect += OnDisconnect;
            AirConsole.instance.onMessage += OnMessage;

            SceneManager.LoadSceneAsync("lobby", LoadSceneMode.Additive);
        }

        protected override void OnDestroy()
        {
            if(null != AirConsole.instance) {
                AirConsole.instance.onMessage -= OnMessage;
                AirConsole.instance.onDisconnect -= OnDisconnect;
                AirConsole.instance.onConnect -= OnConnect;
            }
        }
#endregion

        private bool LoadGameData()
        {
            Debug.Log("Loading game data...");
            //Debug.Log(GameDataFile.text);

// TODO: show an error dialog
            _gameData = JsonUtility.FromJson<GameData>(GameDataFile.text);
            if(null == _gameData) {
                Debug.LogError("There was an error loading the game data!");
                return false;
            }

            _gameData.DebugDump();

            return true;
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
