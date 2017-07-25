using CatFight.Data;
using CatFight.Fighters.Loadouts;
using CatFight.Players;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters
{
    [RequireComponent(typeof(PlayMakerFSM))]
    public sealed class Fighter : MonoBehavior
    {
        [SerializeField]
        [ReadOnly]
        private Player.TeamIds _teamId;

        public Player.TeamIds TeamId => _teamId;

        [SerializeField]
        [ReadOnly]
        private Loadout _loadout;

        [SerializeField]
        [ReadOnly]
        private FighterStats _stats;

        private PlayMakerFSM _fsm;

#region Unity Lifecycle
        private void Awake()
        {
            _fsm = GetComponent<PlayMakerFSM>();
        }
#endregion

        public void Initialize(Player.TeamIds teamId, FighterData fighterData)
        {
            _teamId = teamId;

            _loadout = new Loadout(this);
            _loadout.BuildLoadout();

            _stats = new FighterStats(this, fighterData);
            _stats.Compile(_loadout, _fsm);
        }
    }
}
