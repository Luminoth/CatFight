using CatFight.Util;

using UnityEngine;

namespace CatFight
{
    public sealed class Fighter : MonoBehavior
    {
        [SerializeField]
        [ReadOnly]
        private PlayerTeam.TeamIds _teamId;

        public void Initialize(PlayerTeam.TeamIds teamId)
        {
            _teamId = teamId;

            // TODO: get the completed schematic for the team and init all our stuff
        }
    }
}
