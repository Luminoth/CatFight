using System.Collections;

using CatFight.AirConsole;
using CatFight.Audio;
using CatFight.Data;
using CatFight.Fighters;
using CatFight.Players;
using CatFight.Stages;
using CatFight.Util;
using CatFight.Util.ObjectPool;

using UnityEngine;

namespace CatFight.Loading
{
    public sealed class LoadingManager : SingletonBehavior<LoadingManager>
    {
        [SerializeField]
        private LoadingScreen _loadingScreen;

#region Manager Prefabs
        [SerializeField]
        private ObjectPoolManager _objectPoolManagerPrefab;

        [SerializeField]
        private DataManager _dataManagerPrefab;

        [SerializeField]
        private GameStageManager _gameStageManagerPrefab;

        [SerializeField]
        private FighterManager _fighterManagerPrefab;

        [SerializeField]
        private AudioManager _audioManagerPrefab;
#endregion

        [SerializeField]
        [ReadOnly]
        private GameObject _managersObject;

        [SerializeField]
        [ReadOnly]
        private bool _isAirConsoleReady;

#region Unity Lifecycle
        private void Start()
        {
            AirConsoleManager.Instance.ReadyEvent += ReadyEventHandler;

            StartCoroutine(Load());
        }

        protected override void OnDestroy()
        {
            if(AirConsoleManager.HasInstance) {
                AirConsoleManager.Instance.ReadyEvent -= ReadyEventHandler;
            }
        }
#endregion

        private IEnumerator Load()
        {
            _loadingScreen.Progress.Percent = 0.0f;
            _loadingScreen.ProgressText = "Creating Managers...";
            yield return null;

            CreateManagers();
            yield return null;

            _loadingScreen.Progress.Percent = 0.5f;
            _loadingScreen.ProgressText = "Waiting for AirConsole...";
            yield return null;

            while(!_isAirConsoleReady) {
                yield return null;
            }

            _loadingScreen.Progress.Percent = 0.75f;
            _loadingScreen.ProgressText = "Loading lobby...";
            GameStageManager.Instance.LoadLobby(() => {
                _loadingScreen.Progress.Percent = 1.0f;
                _loadingScreen.ProgressText = "Loading complete!";

                Destroy();
            });
        }

        private void CreateManagers()
        {
            _managersObject = new GameObject("Managers");

            ObjectPoolManager.CreateFromPrefab(_objectPoolManagerPrefab.gameObject, _managersObject);
            DataManager.CreateFromPrefab(_dataManagerPrefab.gameObject, _managersObject);
            GameStageManager.CreateFromPrefab(_gameStageManagerPrefab.gameObject, _managersObject);
            FighterManager.CreateFromPrefab(_fighterManagerPrefab.gameObject, _managersObject);
            AudioManager.CreateFromPrefab(_audioManagerPrefab.gameObject, _managersObject);
            PlayerManager.Create(_managersObject);
        }

        private void Destroy()
        {
            Destroy(_loadingScreen.gameObject);
            _loadingScreen = null;

            Destroy(gameObject);
        }

#region Event Handlers
        private void ReadyEventHandler(object sender, ReadyEventArgs evt)
        {
            _isAirConsoleReady = true;
        }
#endregion
    }
}
