using CatFight.Util;

using UnityEngine;

namespace CatFight.Stages.Arena
{
    public sealed class ArenaEdge : MonoBehavior
    {
        [SerializeField]
        private bool _isGround;

        public bool IsGround => _isGround;
    }
}
