using CatFight.Players;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Stages.Arena
{
    public sealed class FighterSpawn : MonoBehavior
    {
        [SerializeField]
        private Player.TeamIds _teamId = Player.TeamIds.None;

        public Player.TeamIds TeamId => _teamId;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 1.0f);
        }
#endif
    }
}
