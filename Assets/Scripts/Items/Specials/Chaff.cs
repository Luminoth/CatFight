using System;

using CatFight.Fighters;
using CatFight.Util.ObjectPool;

using UnityEngine;

namespace CatFight.Items.Specials
{
    [RequireComponent(typeof(PooledObject))]
    public sealed class Chaff : SpecialAmmo
    {
        private PooledObject _pooledObject;

        private IChaffTarget _target;

#region Unity Lifecycle
        private void Awake()
        {
            _pooledObject = GetComponent<PooledObject>();
            _pooledObject.RecycleEvent += RecycleEventHandler;
        }
#endregion

        public override void Initialize(Fighter fighter)
        {
            base.Initialize(fighter);

            fighter.Stats.AddChaff(this);

// TODO: chaff should fly up and then fall down
        }

        protected override void Destroy()
        {
            _target?.OnChaffDied();

            Fighter.Stats.RemoveChaff(this);

            base.Destroy();
        }

        public void Use(IChaffTarget target)
        {
            Fighter.Stats.RemoveChaff(this);

            _target = target;
        }

        public void Release()
        {
            Fighter.Stats.AddChaff(this);

            _target = null;
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
