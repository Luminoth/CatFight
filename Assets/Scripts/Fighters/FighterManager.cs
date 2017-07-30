using System.Collections;
using System.Collections.Generic;
using System.Linq;

using CatFight.Data;
using CatFight.Items.Weapons;
using CatFight.Players;
using CatFight.Util;
using CatFight.Util.ObjectPool;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Fighters
{
    public sealed class FighterManager : SingletonBehavior<FighterManager>
    {
        [SerializeField]
        private Fighter _fighterPrefab;

        private GameObject _fighterContainer;

        public GameObject AmmoContainer { get; private set; }

        private readonly Dictionary<Player.TeamIds, Fighter> _fighters = new Dictionary<Player.TeamIds, Fighter>();

        public IReadOnlyDictionary<Player.TeamIds, Fighter> Fighters => _fighters;

#region Unity Lifecycle
        private void Awake()
        {
            AmmoContainer = new GameObject("Ammo");
            AmmoContainer.transform.SetParent(transform);

            _fighterContainer = new GameObject("Fighters");
            _fighterContainer.transform.SetParent(transform);
        }
#endregion

        public void InitFighters(IReadOnlyCollection<FighterSpawn> spawnPoints)
        {
            var fighterNames = new List<string>();
            DataManager.Instance.GameData.Fighter.GetRandomFighterNames(fighterNames, spawnPoints.Count);

            for(int i=0; i<spawnPoints.Count; ++i) {
                FighterSpawn spawnPoint = spawnPoints.ElementAt(i);

                Fighter fighter = SpawnFighter(spawnPoint);
                _fighters.Add(spawnPoint.TeamId, fighter);

                fighter.Initialize(spawnPoint.TeamId, fighterNames[i], DataManager.Instance.GameData.Fighter);
            }
        }

        public void Cleanup()
        {
            RecycleAmmo();
            DestroyFighters();

// TODO: we can do this when RecycleAmmo actually works
            //StopAllCoroutines();
        }

        private void RecycleAmmo()
        {
// TODO: this isn't working for some reason
            for(int i=0; i<AmmoContainer.transform.childCount; ++i) {
                Transform child = AmmoContainer.transform.GetChild(i);
                PooledObject pooledObject = child.GetComponent<PooledObject>();
                if(null != pooledObject) {
                    pooledObject.Recycle();
                }
            }
        }

        private void DestroyFighters()
        {
            // TODO: make this a Transform extension
            for(int i=0; i<_fighterContainer.transform.childCount; ++i) {
                Transform child = _fighterContainer.transform.GetChild(i);
                Destroy(child.gameObject);
            }

            _fighters.Clear();
        }

        [CanBeNull]
        public Fighter GetFighter(Player.TeamIds teamId)
        {
            return _fighters.GetOrDefault(teamId);
        }

        private Fighter SpawnFighter(FighterSpawn spawnPoint)
        {
            Fighter fighter = Instantiate(_fighterPrefab, _fighterContainer.transform);
            fighter.transform.position = spawnPoint.transform.position;
            return fighter;
        }

        private IEnumerator ImpactCoroutine(PooledObject impactObject)
        {
            // TODO: really we need to wait for N iterations of the animation
            yield return new WaitForSeconds(1);

            impactObject.Recycle();
        }

        public void SpawnImpact(WeaponData.WeaponType weaponType, Impact impactPrefab, Vector3 position, Quaternion rotation)
        {
            PooledObject pooledObject = ObjectPoolManager.Instance.GetPooledObject(WeaponData.GetImpactPool(weaponType), AmmoContainer.transform);
            Impact impact = pooledObject?.GetComponent<Impact>();
            if(null == impact) {
                return;
            }

            impact.transform.position = position;
            impact.transform.rotation = rotation;

            StartCoroutine(ImpactCoroutine(pooledObject));
        }
    }
}
