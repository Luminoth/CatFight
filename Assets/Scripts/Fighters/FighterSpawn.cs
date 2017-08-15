using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters
{
    public sealed class FighterSpawn : MonoBehavior
    {
        [SerializeField]
        private Direction _facingDirection;

        public Direction FacingDirection => _facingDirection;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 1.0f);

            // TODO: make this an extension
            Color oldColor = Gizmos.color;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right.FacingDirection2D(FacingDirection) * 5.0f);
            Gizmos.color = oldColor;
        }
#endif
    }
}
