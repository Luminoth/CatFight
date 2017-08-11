using System;

using CatFight.Util.ObjectPool;

using UnityEngine;

namespace CatFight.Items.Specials
{
    [RequireComponent(typeof(PooledObject))]
    public sealed class Chaff : SpecialAmmo
    {
        private PooledObject _pooledObject;

#region Unity Lifecycle
        private void Awake()
        {
            _pooledObject = GetComponent<PooledObject>();
            _pooledObject.RecycleEvent += RecycleEventHandler;
        }
#endregion

        protected override void OnFighterCollision()
        {
            base.OnFighterCollision();

            _pooledObject.Recycle();
        }

        protected override void OnArenaCollision()
        {
            base.OnArenaCollision();

            _pooledObject.Recycle();
        }

#region Event Handlers
        private void RecycleEventHandler(object sender, EventArgs evt)
        {
            Destroy();
        }
#endregion
    }
}
