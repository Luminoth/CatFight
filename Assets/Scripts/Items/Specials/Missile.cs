using System;

using CatFight.Data;
using CatFight.Fighters;
using CatFight.Stages.Arena;
using CatFight.Util;
using CatFight.Util.ObjectPool;

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

#region Unity Lifecycle
        private void Awake()
        {
            _pooledObject = GetComponent<PooledObject>();
            _pooledObject.RecycleEvent += RecycleEventHandler;

            _rigidBody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
// TODO: missile should track the target
        }
#endregion

        public override void Initialize(Fighter fighter, SpecialData.SpecialType specialType, int damage)
        {
            base.Initialize(fighter, specialType, damage);

            _rigidBody.velocity = transform.right * _velocity;

// TODO: if the target already has a chaff, we should target it instead of the target

// TODO: hook into the target's chaff event (don't forget to unhook!) - when the event fires, we re-target
        }

        protected override void Destroy()
        {
            _fighterTarget = null;

            _chaffTarget?.Release();
            _chaffTarget = null;

            base.Destroy();
        }

        public void OnChaffDied()
        {
            _chaffTarget = null;
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
            _chaffTarget = evt.Chaff;

            evt.Chaff.Use(this);
        }
#endregion
    }
}
