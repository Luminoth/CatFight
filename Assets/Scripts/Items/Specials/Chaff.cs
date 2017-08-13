using System;

using CatFight.Data;
using CatFight.Fighters;
using CatFight.Stages.Arena;
using CatFight.Util.ObjectPool;

using UnityEngine;

namespace CatFight.Items.Specials
{
    [RequireComponent(typeof(PooledObject))]
    public sealed class Chaff : SpecialAmmo
    {
        [SerializeField]
        private GameObject _missileTargetObject;

        private PooledObject _pooledObject;

        private IChaffTarget _target;

#region Unity Lifecycle
        private void Awake()
        {
            _pooledObject = GetComponent<PooledObject>();
            _pooledObject.RecycleEvent += RecycleEventHandler;
        }
#endregion

        public override void Initialize(Fighter fighter, SpecialData.SpecialType specialType, int damage)
        {
            base.Initialize(fighter, specialType, damage);

            fighter.Stats.AddChaff(this);

// TODO: chaff should fly up and then fall down
        }

        protected override void Destroy()
        {
            _target?.OnChaffDied();

            Fighter.Stats.RemoveChaff(this);

            //_missileTargetObject.SetActive(false);

            base.Destroy();
        }

        public void Use(IChaffTarget target)
        {
            Fighter.Stats.RemoveChaff(this);
            //_missileTargetObject.SetActive(true);

            _target = target;
        }

        public void Release()
        {
            //_missileTargetObject.SetActive(false);
            Fighter.Stats.AddChaff(this);

            _target = null;
        }

        protected override void OnArenaCollision(ArenaEdge edge)
        {
            base.OnArenaCollision(edge);

            if(edge.IsGround) {
                _pooledObject.Recycle();
            }
        }

#region Event Handlers
        private void RecycleEventHandler(object sender, EventArgs evt)
        {
            Destroy();
        }
#endregion
    }
}
