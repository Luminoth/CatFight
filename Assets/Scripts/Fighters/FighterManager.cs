using System.Collections.Generic;

using CatFight.Data;
using CatFight.Players;
using CatFight.Util;

using JetBrains.Annotations;

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

        private readonly Dictionary<Player.TeamIds, Fighter> _fighters = new Dictionary<Player.TeamIds, Fighter>();

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
                _fighters.Add(spawnPoint.TeamId, fighter);

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
    }
}
