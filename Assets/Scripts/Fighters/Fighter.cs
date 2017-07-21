using CatFight.Fighters.Loadouts;
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

        public PlayerTeam.TeamIds TeamId => _teamId;

        private Loadout _loadout;

        public void Initialize(PlayerTeam.TeamIds teamId)
        {
            _teamId = teamId;

            _loadout = new Loadout(this);
            _loadout.BuildLoadout();
        }
    }
}
