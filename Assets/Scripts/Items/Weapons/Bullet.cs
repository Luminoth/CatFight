using CatFight.Util.ObjectPool;

using UnityEngine;

namespace CatFight.Items.Weapons
{
    [RequireComponent(typeof(PooledObject))]
    public sealed class Bullet : Ammo
    {
        private PooledObject _pooledObject;

#region Unity Lifecycle
        private void Awake()
        {
            _pooledObject = GetComponent<PooledObject>();
        }

        private void OnBecameInvisible()
        {
            _pooledObject.Recycle();
        }
#endregion
    }
}
