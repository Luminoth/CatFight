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

        public Loadout Loadout => _loadout;

        [SerializeField]
        [ReadOnly]
        private FighterStats _stats;

        public FighterStats Stats => _stats;

        private PlayMakerFSM _fsm;

#region Unity Lifecycle
        private void Awake()
        {
            _fsm = GetComponent<PlayMakerFSM>();

            _loadout = new Loadout(this);
            _stats = new FighterStats(this);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.Space)) {
                Stats.FireAllWeapons();
            }

            // NOTE: assumes missiles are special 1
            if(Input.GetKeyDown(KeyCode.X)) {
                Stats.UseSpecial(1);
            }

            // NOTE: assumes chaffs are special 2
            if(Input.GetKeyDown(KeyCode.C)) {
                Stats.UseSpecial(2);
            }
#endif
        }
#endregion

        public void Initialize(Player.TeamIds teamId, FighterData fighterData)
        {
            _teamId = teamId;

            _loadout.Initialize();
            Debug.Log(Loadout);

            _stats.Initialize(_loadout, _fsm);
            Debug.Log(Stats);
        }
    }
}
