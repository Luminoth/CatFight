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

        public Fighter Fighter { get; private set; }

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
                OnFighterCollision(fighter);
            } else if(null != other.GetComponent<ArenaEdge>()) {
                OnArenaCollision();
            }
        }
#endregion

        public virtual void Initialize(Fighter fighter, int slotId, WeaponData.WeaponType weaponType, int damage)
        {
            Fighter = fighter;

            gameObject.layer = fighter.gameObject.layer;

            WeaponType = weaponType;
            Damage = damage;

            Transform spawn = fighter.GetWeaponAmmoSpawnTransform(slotId);
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }

        protected virtual void Destroy()
        {
            Fighter = null;

            gameObject.layer = _defaultLayer;

            WeaponType = WeaponData.WeaponType.None;
            Damage = 0;

            transform.position = Vector3.zero;
            transform.eulerAngles = Vector3.zero;
        }

        protected virtual void OnFighterCollision(Fighter fighter)
        {
            FighterManager.Instance.SpawnImpact(WeaponType, ImpactPrefab, transform.position, transform.rotation);
        }

        protected virtual void OnArenaCollision()
        {
            FighterManager.Instance.SpawnImpact(WeaponType, ImpactPrefab, transform.position, transform.rotation);
        }
    }
}
