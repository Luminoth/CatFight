using CatFight.Data;
using CatFight.Fighters;
using CatFight.Items.Weapons;
using CatFight.Stages.Arena;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Items.Specials
{
    public abstract class SpecialAmmo : MonoBehavior
    {
        public SpecialData.SpecialType SpecialType { get; private set; } = SpecialData.SpecialType.None;

        [SerializeField]
        private int _damage;

        public int Damage => _damage;

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
                fighter.Stats.SpecialDamage(Damage);
                OnFighterCollision(fighter);
            } else if(null != other.GetComponent<ArenaEdge>()) {
                OnArenaCollision();
            }
        }
#endregion

        public virtual void Initialize(Fighter fighter)
        {
            Fighter = fighter;

            gameObject.layer = Fighter.gameObject.layer;

            Transform spawn = Fighter.GetSpecialAmmoSpawnTransform();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }

        protected virtual void Destroy()
        {
            Fighter = null;

            gameObject.layer = _defaultLayer;

            transform.position = Vector3.zero;
            transform.eulerAngles = Vector3.zero;
        }

        protected virtual void OnFighterCollision(Fighter fighter)
        {
            FighterManager.Instance.SpawnImpact(SpecialType, ImpactPrefab, transform.position, transform.rotation);
        }

        protected virtual void OnArenaCollision()
        {
            FighterManager.Instance.SpawnImpact(SpecialType, ImpactPrefab, transform.position, transform.rotation);
        }
    }
}
