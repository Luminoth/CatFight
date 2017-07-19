using System;
using System.Collections;

using CatFight.AirConsole.Messages;
using CatFight.Util;

using Newtonsoft.Json.Linq;

using UnityEngine;
using UnityEngine.Networking;

namespace CatFight.AirConsole
{
    public sealed class AirConsoleManager : SingletonBehavior<AirConsoleManager>
    {
#region Events
        public event EventHandler<ReadyEventArgs> ReadyEvent;
        public event EventHandler<ConnectEventArgs> ConnectEvent;
        public event EventHandler<DisconnectEventArgs> DisconnectEvent;
        public event EventHandler<CustomDeviceStateChangeEventArgs> CustomDeviceStateEvent;
        public event EventHandler<MessageEventArgs> MessageEvent;
#endregion

        public const string ViewLobby = "lobby";
        public const string ViewStaging = "staging";
        public const string ViewArena = "arena";

        private const string DeviceStateMasterPlayerKey = "masterPlayer";
        private const string DeviceStateScreenViewKey = "screen_view";
        private const string DeviceStateControlViewKey = "ctrl_view";

        private Action _showAdCallback;

#region Unity Lifecycle
        private void Start()
        {
            NDream.AirConsole.AirConsole.instance.onReady += OnReady;
            NDream.AirConsole.AirConsole.instance.onConnect += OnConnect;
            NDream.AirConsole.AirConsole.instance.onDisconnect += OnDisconnect;
            NDream.AirConsole.AirConsole.instance.onMessage += OnMessage;
            NDream.AirConsole.AirConsole.instance.onCustomDeviceStateChange += OnCustomDeviceStateChange;
            NDream.AirConsole.AirConsole.instance.onAdComplete += OnAdComplete;
        }

        protected override void OnDestroy()
        {
            if(null != NDream.AirConsole.AirConsole.instance) {
                NDream.AirConsole.AirConsole.instance.onAdComplete -= OnAdComplete;
                NDream.AirConsole.AirConsole.instance.onCustomDeviceStateChange -= OnCustomDeviceStateChange;
                NDream.AirConsole.AirConsole.instance.onMessage -= OnMessage;
                NDream.AirConsole.AirConsole.instance.onDisconnect -= OnDisconnect;
                NDream.AirConsole.AirConsole.instance.onConnect -= OnConnect;
                NDream.AirConsole.AirConsole.instance.onReady -= OnReady;
            }
        }
#endregion

        public string GetNickname(int deviceId)
        {
            return NDream.AirConsole.AirConsole.instance.GetNickname(deviceId);
        }

        public void GetProfilePicture(int deviceId, Action<Texture2D> callback)
        {
            string profilePictureUrl = NDream.AirConsole.AirConsole.instance.GetProfilePicture(deviceId);
            StartCoroutine(DownloadProfilePicture(profilePictureUrl, callback));
        }

        public void SetMasterPlayer(int deviceId)
        {
            Debug.Log($"Promoting player {deviceId} to master player!");
            SetCustomDeviceStateProperty(DeviceStateMasterPlayerKey, deviceId);
        }

#region Views
        public void SetScreenView(string view)
        {
            SetCustomDeviceStateProperty(DeviceStateScreenViewKey, view);
        }

        public void SetControllerView(string view)
        {
            SetCustomDeviceStateProperty(DeviceStateControlViewKey, view);
        }

        public void SetView(string view)
        {
            SetScreenView(view);
            SetControllerView(view);
        }
#endregion

        public void SetCustomDeviceStateProperty(string key, object value)
        {
            NDream.AirConsole.AirConsole.instance.SetCustomDeviceStateProperty(key, value);
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

            ReadyEvent?.Invoke(this, new ReadyEventArgs());
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

        private void OnCustomDeviceStateChange(int deviceId, JToken customDeviceData)
        {
            Debug.Log($"OnCustomDeviceStateChange({deviceId}, {customDeviceData})");

            CustomDeviceStateEvent?.Invoke(this, new CustomDeviceStateChangeEventArgs(deviceId, customDeviceData));
        }

        private void OnMessage(int from, JToken data)
        {
            Debug.Log($"OnMessage({from}, {data})");

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
