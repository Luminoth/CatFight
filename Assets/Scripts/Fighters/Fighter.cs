using CatFight.Players;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters
{
    public sealed class Fighter : MonoBehavior
    {
        [SerializeField]
        [ReadOnly]
        private PlayerTeam.TeamIds _teamId;

        private Loadout _loadout;

        public void Initialize(PlayerTeam.TeamIds teamId)
        {
            _teamId = teamId;
            _loadout = new Loadout(PlayerManager.Instance.GetTeam(teamId));
        }
    }
}
