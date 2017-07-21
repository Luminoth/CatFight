using CatFight.AirConsole;
using CatFight.Util;

using UnityEngine;
using UnityEngine.UI;

namespace CatFight.Stages.Lobby
{
    public sealed class LobbyPlayer : MonoBehavior
    {
        [SerializeField]
        private GameObject _connectedState;

        [SerializeField]
        private GameObject _disconnectedState;

        [SerializeField]
        private GameObject _masterPlayerState;

        [SerializeField]
        private Text _name;

        [SerializeField]
        private Text _team;

        [SerializeField]
        [ReadOnly]
        private Texture _defaultProfileImage;

        [SerializeField]
        private RawImage _profileImage;

        public Player Player { get; private set; }

        public string Name
        {
            get { return _name.text; }

            private set { _name.text = value; }
        }

        public string Team
        {
            get { return _team.text; }

            set { _team.text = value; }
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

#region Unity Lifecycle
        private void Awake()
        {
            _defaultProfileImage = _profileImage?.texture;
        }
#endregion

        public void Initialize(Player player)
        {
            Player = player;

            Name = AirConsoleManager.Instance.GetNickname(Player.DeviceId);
            Team = Player.Team.Id.GetDescription();
            AirConsoleManager.Instance.GetProfilePicture(Player.DeviceId, profileImage => {
                ProfileImage = profileImage;
            });
        }

        public void SetConnected(bool isConnected)
        {
            _connectedState.SetActive(isConnected);
            _disconnectedState.SetActive(!isConnected);
        }
    }
}
