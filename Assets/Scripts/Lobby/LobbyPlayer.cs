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

        [SerializeField]
        [ReadOnly]
        private Texture _defaultProfileImage;

        [SerializeField]
        private RawImage _profileImage;

        public string Name
        {
            get { return _name.text; }

            set
            {
                _name.text = value;
            }
        }

        public Texture ProfileImage
        {
            get { return _profileImage?.texture; }

            set
            {
                if(null != _profileImage) {
                    _profileImage.texture = value ?? _defaultProfileImage;
                }
            }
        }

        private void Awake()
        {
            _defaultProfileImage = _profileImage?.texture;
        }

        public void SetConnected(bool isConnected)
        {
            _connectedState.SetActive(isConnected);
            _disconnectedState.SetActive(!isConnected);
        }
    }
}
