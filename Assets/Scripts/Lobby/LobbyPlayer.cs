using CatFight.Util;

using UnityEngine;

namespace CatFight.Lobby
{
    public sealed class LobbyPlayer : MonoBehavior
    {
        [SerializeField]
        private GameObject _connectedState;

        [SerializeField]
        private GameObject _disconnectedState;

        public void SetConnected(bool isConnected)
        {
            _connectedState.SetActive(isConnected);
            _disconnectedState.SetActive(!isConnected);
        }
    }
}
