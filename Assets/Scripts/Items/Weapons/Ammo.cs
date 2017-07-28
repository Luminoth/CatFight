using CatFight.Data;
using CatFight.Fighters;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Items.Weapons
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Ammo : MonoBehavior
    {
        public WeaponData.WeaponType WeaponType { get; private set; }

        public int Damage { get; private set; }

        private Collider2D _collider;

        private Fighter _fighter;

#region Unity Lifecycle
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnDisable()
        {
            if(null != _fighter) {
                Physics2D.IgnoreCollision(_collider, _fighter.Collider, false);
            }
            _fighter = null;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Fighter fighter = collision.gameObject.GetComponent<Fighter>();
            if(null != fighter && fighter != _fighter) {
                fighter.Stats.Damage(Damage, WeaponType);
                OnFighterCollision();
            }
        }
#endregion

        public virtual void Initialize(Fighter fighter, WeaponData.WeaponType weaponType, int damage)
        {
            _fighter = fighter;
            Physics2D.IgnoreCollision(_collider, _fighter.Collider);

            WeaponType = weaponType;
            Damage = damage;

            transform.position = _fighter.transform.position;
            transform.rotation = _fighter.transform.rotation;
        }

        protected virtual void OnFighterCollision()
        {
        }
    }
}
