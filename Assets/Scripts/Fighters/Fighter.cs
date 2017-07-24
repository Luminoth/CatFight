using CatFight.AirConsole.Messages;
using CatFight.Fighters.Loadouts;
using CatFight.Players;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters
{
    public sealed class Fighter : MonoBehavior
    {

        public bool Up = false;
        public bool Down = false;
        public bool Left = false;
        public bool Right = false;
        public bool Fire = false;
        // -1 = null
        public int FireType = -1;


        [SerializeField]
        [ReadOnly]
        private PlayerTeam.TeamIds _teamId;

        public PlayerTeam.TeamIds TeamId => _teamId;

        private Loadout _loadout;

        public void Initialize(PlayerTeam.TeamIds teamId)
        {
            _teamId = teamId;

            _loadout = new Loadout(this);
            _loadout.BuildLoadout();
        }

        public void Input(SetInputMessage InputData)
        {
            switch (InputData.inputButton)
            {
                case SetInputMessage.inputButtons.up:
                    Up = InputData.buttonState;
                    break;
                case SetInputMessage.inputButtons.down:
                    Down = InputData.buttonState;
                    break;
                case SetInputMessage.inputButtons.left:
                    Left = InputData.buttonState;
                    break;
                case SetInputMessage.inputButtons.right:
                    Right = InputData.buttonState;
                    break;
                case SetInputMessage.inputButtons.fire:
                    Fire = InputData.buttonState;
                    FireType = InputData.fireType;
                    if (!InputData.buttonState) FireType = -1;
                    break;

            }

            Debug.Log(TeamId + " inputs: " + InputData.ToString());
        }
    }
}
