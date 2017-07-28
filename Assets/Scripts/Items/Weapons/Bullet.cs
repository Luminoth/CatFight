using CatFight.Data;
using CatFight.Fighters;
using CatFight.Util.ObjectPool;

using UnityEngine;

namespace CatFight.Items.Weapons
{
    [RequireComponent(typeof(PooledObject))]
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Bullet : Ammo
    {
        private PooledObject _pooledObject;

        private Rigidbody2D _rigidBody;

        [SerializeField]
        private float _velocity = 5.0f;

#region Unity Lifecycle
        private void Awake()
        {
            _pooledObject = GetComponent<PooledObject>();
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        private void OnBecameInvisible()
        {
Debug.LogError("invisible");
            _pooledObject.Recycle();
        }
#endregion

        public override void Initialize(Fighter fighter, WeaponData.WeaponType weaponType, int damage)
        {
            base.Initialize(fighter, weaponType, damage);

            _rigidBody.velocity = transform.forward * _velocity;
        }

        protected override void OnFighterCollision()
        {
            base.OnFighterCollision();

            _pooledObject.Recycle();
        }
    }
}
