using CatFight.Data;
using CatFight.Players;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters
{
    public sealed class FighterSpawn : MonoBehavior
    {
        [SerializeField]
        private int _teamId;

        public int TeamId => _teamId;

        public TeamData.TeamDataEntry Team => DataManager.Instance.GameData.Teams.Entries[TeamId];

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 1.0f);
        }
#endif
    }
}
