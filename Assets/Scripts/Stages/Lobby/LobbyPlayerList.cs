using System.Collections.Generic;

using CatFight.Players;
using CatFight.Util;

using UnityEngine;
using UnityEngine.UI;

namespace CatFight.Stages.Lobby
{
    [RequireComponent(typeof(LayoutGroup))]
    public sealed class LobbyPlayerList : MonoBehavior
    {
        [SerializeField]
        private int _maxPlayers = 10;

        public bool IsFull => _players.Count >= _maxPlayers;

        private readonly Dictionary<int, LobbyPlayer> _players = new Dictionary<int, LobbyPlayer>();

        public bool AddPlayer(int deviceId)
        {
            if(IsFull) {
                return false;
            }

            Player player;
            if(!PlayerManager.Instance.Players.TryGetValue(deviceId, out player)) {
                Debug.LogError($"Cannot add non-existant player {deviceId}");
                return false;
            }

            LobbyPlayer lobbyPlayer = Instantiate(Lobby.Instance.LobbyPlayerPrefab, transform);
            lobbyPlayer.Initialize(player);
            lobbyPlayer.SetConnected(true);

            _players.Add(deviceId, lobbyPlayer);

            return true;
        }

        public void ReconnectPlayer(int deviceId)
        {
            LobbyPlayer lobbyPlayer;
            if(!_players.TryGetValue(deviceId, out lobbyPlayer)) {
                Debug.LogError($"Unable to reconnect non-existant player {deviceId}!");
                return;
            }
            lobbyPlayer.SetConnected(true);
        }

        public void DisconnectPlayer(int deviceId)
        {
            LobbyPlayer lobbyPlayer;
            if(!_players.TryGetValue(deviceId, out lobbyPlayer)) {
                Debug.LogError($"Unable to disconnect non-existant player {deviceId}!");
                return;
            }
            lobbyPlayer.SetConnected(false);
        }
    }
}
