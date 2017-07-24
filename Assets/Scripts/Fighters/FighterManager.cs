using System.Collections.Generic;

using CatFight.Data;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters
{
    public sealed class FighterManager : SingletonBehavior<FighterManager>
    {
        [SerializeField]
        private Fighter _fighterPrefab;

        [SerializeField]
        [ReadOnly]
        private GameObject _fighterContainer;

        private readonly List<Fighter> _fighters = new List<Fighter>();

#region Unity Lifecycle
        protected override void OnDestroy()
        {
            DestroyFighters();

            base.OnDestroy();
        }
#endregion

        public void InitFighters(IReadOnlyCollection<FighterSpawn> spawnPoints)
        {
            if(null != _fighterContainer) {
                return;
            }
            _fighterContainer = new GameObject("Fighters");

            foreach(FighterSpawn spawnPoint in spawnPoints) {
                Fighter fighter = SpawnFighter(spawnPoint);
                _fighters.Add(fighter);

                fighter.Initialize(spawnPoint.TeamId, DataManager.Instance.GameData.Fighter);
            }
        }

        public void DestroyFighters()
        {
            if(null == _fighterContainer) {
                return;
            }

            Destroy(_fighterContainer);
            _fighterContainer = null;
        }

        private Fighter SpawnFighter(FighterSpawn spawnPoint)
        {
            Fighter fighter = Instantiate(_fighterPrefab, _fighterContainer.transform);
            fighter.transform.position = spawnPoint.transform.position;
            return fighter;
        }
    }
}
