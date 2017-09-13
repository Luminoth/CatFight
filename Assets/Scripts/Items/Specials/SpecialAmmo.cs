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
                OnFighterCollision(fighter);
                return;
            }

            ArenaEdge edge = other.GetComponent<ArenaEdge>();
            if(null != edge) {
                OnArenaCollision(edge);
                return;
            }
        }
#endregion

        public virtual void Initialize(Fighter fighter, SpecialData.SpecialType specialType, int damage)
        {
            Fighter = fighter;

            gameObject.layer = Fighter.gameObject.layer;

            SpecialType = specialType;
            Damage = damage;

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
            fighter.Stats.SpecialDamage(Damage);

            Impact();
        }

        protected virtual void OnArenaCollision(ArenaEdge edge)
        {
            Impact();
        }

        private void Impact()
        {
            // TODO: play impact audio

            FighterManager.Instance.SpawnImpact(SpecialType, transform.position);
        }
    }
}
