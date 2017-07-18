using CatFight.Util;

using UnityEngine;
using UnityEngine.UI;

namespace CatFight.Lobby
{
    public sealed class LobbyPlayer : MonoBehavior
    {
        [SerializeField]
        private GameObject _connectedState;

        [SerializeField]
        private GameObject _disconnectedState;

        [SerializeField]
        private Text _name;

        public string Name
        {
            get { return _name.text; }

            set
            {
                _name.text = value;
            }
        }

        public void SetConnected(bool isConnected)
        {
            _connectedState.SetActive(isConnected);
            _disconnectedState.SetActive(!isConnected);
        }
    }
}
