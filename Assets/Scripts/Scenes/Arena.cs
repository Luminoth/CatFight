using System;
using System.Collections;

using CatFight.Util;

using UnityEngine;
using UnityEngine.UI;

namespace CatFight.Scenes
{
    public sealed class Arena : SingletonBehavior<Arena>
    {
        [SerializeField]
        private int _countdownSeconds = 5;

        [SerializeField]
        private Text _countdownText;

        [SerializeField]
        private int _fightTimeSeconds = 120;

        [SerializeField]
        private Text _timerText;

        [SerializeField]
        [ReadOnly]
        private int _actualFightTimeSeconds;

        private void Start()
        {
            _countdownText.text = _countdownSeconds.ToString();
            _timerText.text = _fightTimeSeconds.ToString();

            StartCoroutine(Countdown(() => {
                StartCoroutine(Timer(() => {
                    GameStageManager.Instance.LoadLobby();
                }));
            }));
        }

// TODO: these shouldn't be in coroutines, just do them in Update() and track the state

        private IEnumerator Countdown(Action callback)
        {
            DateTime start = DateTime.Now;
            DateTime end = start.AddSeconds(_countdownSeconds);

            TimeSpan remaining = end - DateTime.Now;
            while(remaining.TotalSeconds > 0) {
                _countdownText.text = ((int)remaining.TotalSeconds).ToString();
                yield return new WaitForSeconds(1);
                remaining = end - DateTime.Now;
            }

            callback?.Invoke();
        }

        private IEnumerator Timer(Action callback)
        {
            DateTime start = DateTime.Now;

            _actualFightTimeSeconds = _fightTimeSeconds;
            while(true) {
                DateTime end = start.AddSeconds(_actualFightTimeSeconds);
                TimeSpan remaining = end - DateTime.Now;
                if(remaining.TotalSeconds <= 0) {
                    break;
                }

                _timerText.text = ((int)remaining.TotalSeconds).ToString();
                yield return new WaitForSeconds(1);
            }

            callback?.Invoke();
        }
    }
}
