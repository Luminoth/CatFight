using CatFight.Util;

using UnityEngine;

namespace CatFight.Stages.Arena
{
    public sealed class FighterSpawn : MonoBehavior
    {
        [SerializeField]
        private PlayerTeam.TeamIds _teamId = PlayerTeam.TeamIds.None;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 1.0f);
        }
#endif
    }
}
