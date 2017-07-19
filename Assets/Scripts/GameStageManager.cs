﻿using System;
using System.Collections;
using System.Collections.Generic;

using CatFight.AirConsole;
using CatFight.Util;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatFight
{
    public sealed class GameStageManager : SingletonBehavior<GameStageManager>
    {
        [SerializeField]
        private GameObject _loadingScreen;

        [SerializeField]
        private string _lobbySceneName = "lobby";

        [SerializeField]
        private string _stagingSceneName = "staging";

        [SerializeField]
        private string _arenaSceneName = "arena";

        private readonly List<string> _loadedScenes = new List<string>();

#region Unity Lifecycle
        private void Awake()
        {
            AirConsoleController.Instance.ReadyEvent += ReadyEventHandler;
        }

        protected override void OnDestroy()
        {
            AirConsoleController.Instance.ReadyEvent -= ReadyEventHandler;
        }
#endregion

        public void LoadLobby(Action callback=null)
        {
            UnloadScenes();

            StartCoroutine(LoadSceneRoutine(_lobbySceneName, () => {
                AirConsoleController.Instance.SetView(AirConsoleController.ViewLobby);

                callback?.Invoke();
            }));
        }

        public void LoadStaging(Action callback=null)
        {
            UnloadScenes();

            StartCoroutine(LoadSceneRoutine(_stagingSceneName, () => {
                AirConsoleController.Instance.SetView(AirConsoleController.ViewStaging);

                callback?.Invoke();
            }));
        }

        public void LoadArena(Action callback=null)
        {
            UnloadScenes();

            StartCoroutine(LoadSceneRoutine(_arenaSceneName, () => {
                AirConsoleController.Instance.SetView(AirConsoleController.ViewArena);

                callback?.Invoke();
            }));
        }

        private IEnumerator LoadSceneRoutine(string sceneName, Action callback)
        {
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while(!asyncOp.isDone) {
                yield return null;
            }

            _loadedScenes.Add(sceneName);

            callback?.Invoke();
        }

        private void UnloadScenes()
        {
            foreach(string sceneName in _loadedScenes) {
                SceneManager.UnloadSceneAsync(sceneName);
            }
            _loadedScenes.Clear();
        }

#region Event Handlers
        private void ReadyEventHandler(object sender, ReadyEventArgs evt)
        {
            LoadLobby(() => {
                if(null != _loadingScreen) {
                    Destroy(_loadingScreen);
                    _loadingScreen = null;
                }
            });
        }
#endregion
    }
}
