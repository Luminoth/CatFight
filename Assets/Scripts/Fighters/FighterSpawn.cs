using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters
{
    public sealed class FighterSpawn : MonoBehavior
    {
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 1.0f);
        }
#endif
    }
}
