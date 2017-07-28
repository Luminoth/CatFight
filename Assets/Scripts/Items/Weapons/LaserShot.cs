﻿using System;

using CatFight.Data;
using CatFight.Fighters;
using CatFight.Util.ObjectPool;

using UnityEngine;

namespace CatFight.Items.Weapons
{
    [RequireComponent(typeof(PooledObject))]
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class LaserShot : Ammo
    {
        private PooledObject _pooledObject;

        private Rigidbody2D _rigidBody;

        [SerializeField]
        private float _velocity = 30.0f;

#region Unity Lifecycle
        private void Awake()
        {
            _pooledObject = GetComponent<PooledObject>();
            _pooledObject.RecycleEvent += RecycleEventHandler;

            _rigidBody = GetComponent<Rigidbody2D>();
        }
#endregion

        public override void Initialize(Fighter fighter, WeaponData.WeaponType weaponType, int damage)
        {
            base.Initialize(fighter, weaponType, damage);

            _rigidBody.velocity = transform.right * _velocity;
        }

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
            _rigidBody.velocity = Vector2.zero;

            Destroy();
        }
#endregion
    }
}