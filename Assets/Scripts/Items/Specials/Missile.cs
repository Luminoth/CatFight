using System;

using CatFight.Data;
using CatFight.Fighters;
using CatFight.Stages.Arena;
using CatFight.Util;
using CatFight.Util.ObjectPool;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Items.Specials
{
    [RequireComponent(typeof(PooledObject))]
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Missile : SpecialAmmo, IChaffTarget
    {
        private PooledObject _pooledObject;

        private Rigidbody2D _rigidBody;

        [SerializeField]
        private float _velocity = 25.0f;

        [SerializeField]
        [ReadOnly]
        private Fighter _fighterTarget;

        [SerializeField]
        [ReadOnly]
        private Chaff _chaffTarget;

        [CanBeNull]
        private Transform Target => _chaffTarget?.transform ?? _fighterTarget?.transform;

#region Unity Lifecycle
        private void Awake()
        {
            _pooledObject = GetComponent<PooledObject>();
            _pooledObject.RecycleEvent += RecycleEventHandler;

            _rigidBody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            transform.LookAt2D(Target);
            _rigidBody.velocity = transform.right * _velocity;
        }
#endregion

        public void Initialize(Fighter fighter, Fighter target, SpecialData.SpecialType specialType, int damage)
        {
            base.Initialize(fighter, specialType, damage);

            _fighterTarget = target;

            Chaff chaff = _fighterTarget.Stats.GetChaff();
            if(null != chaff) {
                TargetChaff(chaff);
            } else {
                TargetFighter(target);
            }

            transform.LookAt2D(Target);
            _rigidBody.velocity = transform.right * _velocity;
        }

        public override void Initialize(Fighter fighter, SpecialData.SpecialType specialType, int damage)
        {
            throw new NotSupportedException();
        }

        protected override void Destroy()
        {
            _fighterTarget.Stats.ChaffEvent -= ChaffEventHandler;
            _fighterTarget = null;

            _chaffTarget?.Release();
            _chaffTarget = null;

            base.Destroy();
        }

        public void OnChaffDied()
        {
            _chaffTarget = null;
            TargetFighter(_fighterTarget);
        }

        private void TargetFighter(Fighter fighter)
        {
            _fighterTarget = fighter;
            _fighterTarget.AddMissileTarget();

            _fighterTarget.Stats.ChaffEvent += ChaffEventHandler;
        }

        private void TargetChaff(Chaff chaff)
        {
            _chaffTarget = chaff;
            _chaffTarget.Use(this);
        }

        protected override void OnFighterCollision(Fighter fighter)
        {
            base.OnFighterCollision(fighter);

            _pooledObject.Recycle();
        }

        protected override void OnArenaCollision(ArenaEdge edge)
        {
            base.OnArenaCollision(edge);

            _pooledObject.Recycle();
        }

#region Event Handlers
        private void RecycleEventHandler(object sender, EventArgs evt)
        {
            _rigidBody.velocity = Vector2.zero;

            Destroy();
        }

        private void ChaffEventHandler(object sender, ChaffEventArgs evt)
        {
            if(null != _chaffTarget) {
                return;
            }
            TargetChaff(evt.Chaff);
        }
#endregion
    }
}
