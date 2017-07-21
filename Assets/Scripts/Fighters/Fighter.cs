using CatFight.Data;
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

        [SerializeField]
        private FighterStats _stats;

        public void Initialize(PlayerTeam.TeamIds teamId, FighterData fighterData)
        {
            _teamId = teamId;

            _loadout = new Loadout(this);
            _loadout.BuildLoadout();

            _stats = new FighterStats(this, fighterData);
            _stats.Compile(_loadout);
        }
    }
}
