using System;
using System.Collections;

using CatFight.AirConsole.Messages;
using CatFight.Data;
using CatFight.Util;

using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace CatFight.AirConsole
{
    [RequireComponent(typeof(NDream.AirConsole.AirConsole))]
    public sealed class AirConsoleController : SingletonBehavior<AirConsoleController>
    {
#region Events
        public event EventHandler<ConnectEventArgs> ConnectEvent;
        public event EventHandler<DisconnectEventArgs> DisconnectEvent;
        public event EventHandler<MessageEventArgs> MessageEvent;
#endregion

        [SerializeField]
        private TextAsset _gameDataFile;

        public TextAsset GameDataFile => _gameDataFile;

        public GameData GameData { get; private set; }

        [SerializeField]
        private string _lobbySceneName = "lobby";

        private Action _showAdCallback;

#region Unity Lifecycle
        private void Start()
        {
            if(!LoadGameData()) {
                return;
            }

            NDream.AirConsole.AirConsole.instance.onReady += OnReady;
            NDream.AirConsole.AirConsole.instance.onConnect += OnConnect;
            NDream.AirConsole.AirConsole.instance.onDisconnect += OnDisconnect;
            NDream.AirConsole.AirConsole.instance.onMessage += OnMessage;
            NDream.AirConsole.AirConsole.instance.onAdComplete += OnAdComplete;

            SceneManager.LoadSceneAsync(_lobbySceneName, LoadSceneMode.Additive);
        }

        protected override void OnDestroy()
        {
            if(null != NDream.AirConsole.AirConsole.instance) {
                NDream.AirConsole.AirConsole.instance.onAdComplete -= OnAdComplete;
                NDream.AirConsole.AirConsole.instance.onMessage -= OnMessage;
                NDream.AirConsole.AirConsole.instance.onDisconnect -= OnDisconnect;
                NDream.AirConsole.AirConsole.instance.onConnect -= OnConnect;
                NDream.AirConsole.AirConsole.instance.onReady -= OnReady;
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

        public string GetNickname(int deviceId)
        {
            return NDream.AirConsole.AirConsole.instance.GetNickname(deviceId);
        }

        public void GetProfilePicture(int deviceId, Action<Texture2D> callback)
        {
            string profilePictureUrl = NDream.AirConsole.AirConsole.instance.GetProfilePicture(deviceId);
            StartCoroutine(DownloadProfilePicture(profilePictureUrl, callback));
        }

        public void Message(int to, Message message)
        {
            NDream.AirConsole.AirConsole.instance.Message(to, message);
        }

        public void Broadcast(Message message)
        {
            NDream.AirConsole.AirConsole.instance.Broadcast(message);
        }

        public void ShowAd(Action callback)
        {
            NDream.AirConsole.AirConsole.instance.ShowAd();
            _showAdCallback += callback;
        }

        private IEnumerator DownloadProfilePicture(string profilePictureUrl, Action<Texture2D> callback)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(profilePictureUrl);
            yield return request.Send();

            if(request.isHttpError || request.isNetworkError) {
                yield break;
            }

            callback(((DownloadHandlerTexture)request.downloadHandler).texture);
        }

#region Event Handlers
        private void OnReady(string code)
        {
            Debug.Log($"OnReady({code})");
        }

        private void OnConnect(int deviceId)
        {
            Debug.Log($"OnConnect({deviceId})");

            bool isReconnect;
            PlayerManager.Instance.ConnectPlayer(deviceId, out isReconnect);

            ConnectEvent?.Invoke(this, new ConnectEventArgs(deviceId, isReconnect));
        }

        private void OnDisconnect(int deviceId)
        {
            Debug.Log($"OnDisconnect({deviceId})");

            PlayerManager.Instance.DisconnectPlayer(deviceId);

            DisconnectEvent?.Invoke(this, new DisconnectEventArgs(deviceId));
        }

        private void OnMessage(int from, JToken data)
        {
            Debug.Log($"OnMessage({from}: {data})");

            MessageEvent?.Invoke(this, new MessageEventArgs(from, data));
        }

        private void OnAdComplete(bool adWasShown)
        {
            Debug.Log($"OnAdComplete({adWasShown})");

            _showAdCallback?.Invoke();
            _showAdCallback = null;
        }
#endregion
    }
}
