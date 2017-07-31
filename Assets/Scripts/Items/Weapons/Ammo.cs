using CatFight.Data;
using CatFight.Fighters;
using CatFight.Stages.Arena;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Items.Weapons
{
    public abstract class Ammo : MonoBehavior
    {
        public WeaponData.WeaponType WeaponType { get; private set; } = WeaponData.WeaponType.None;

        public int Damage { get; private set; }

        [SerializeField]
        private Impact _impactPrefab;

        public Impact ImpactPrefab => _impactPrefab;

        private int _defaultLayer;

#region Unity Lifecycle
        private void Start()
        {
            _defaultLayer = gameObject.layer;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Fighter fighter = other.GetComponent<Fighter>();
            if(null != fighter) {
                fighter.Stats.Damage(Damage, WeaponType);
                OnFighterCollision();
            } else if(null != other.GetComponent<ArenaEdge>()) {
                OnArenaCollision();
            }
        }
#endregion

        public virtual void Initialize(Fighter fighter, WeaponData.WeaponType weaponType, int damage)
        {
            gameObject.layer = fighter.gameObject.layer;

            WeaponType = weaponType;
            Damage = damage;

            transform.position = fighter.transform.position;
            transform.rotation = fighter.transform.rotation;
        }

        protected void Destroy()
        {
            gameObject.layer = _defaultLayer;

            WeaponType = WeaponData.WeaponType.None;
            Damage = 0;

            transform.position = Vector3.zero;
            transform.eulerAngles = Vector3.zero;
        }

        protected virtual void OnFighterCollision()
        {
            FighterManager.Instance.SpawnImpact(WeaponType, ImpactPrefab, transform.position, transform.rotation);
        }

        protected virtual void OnArenaCollision()
        {
            FighterManager.Instance.SpawnImpact(WeaponType, ImpactPrefab, transform.position, transform.rotation);
        }
    }
}
