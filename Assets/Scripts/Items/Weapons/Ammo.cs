using CatFight.Data;
using CatFight.Fighters;
using CatFight.Players;
using CatFight.Stages.Arena;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Items.Weapons
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Ammo : MonoBehavior
    {
        public WeaponData.WeaponType WeaponType { get; private set; } = WeaponData.WeaponType.None;

        public int Damage { get; private set; }

        [SerializeField]
        private Impact _impactPrefab;

        public Impact ImpactPrefab => _impactPrefab;

        private int _defaultLayer;

        private Collider2D _collider;

        private Player.TeamIds _teamId = Player.TeamIds.None;

#region Unity Lifecycle
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

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
            _teamId = fighter.TeamId;
            gameObject.layer = fighter.gameObject.layer;

            WeaponType = weaponType;
            Damage = damage;

            transform.position = fighter.transform.position;
            transform.rotation = fighter.transform.rotation;
        }

        protected void Destroy()
        {
            _teamId = Player.TeamIds.None;
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
