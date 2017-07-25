using System;

using CatFight.AirConsole;
using CatFight.Fighters;
using CatFight.Util;

using UnityEngine;
using UnityEngine.UI;

namespace CatFight.Stages.Arena
{
    public sealed class Arena : Stage
    {
        public new static Arena Instance => (Arena)Stage.Instance;

#region Countdown
        [SerializeField]
        private int _countdownSeconds = 5;

        [SerializeField]
        private GameObject _countdownContainer;

        [SerializeField]
        private Text _countdownText;

        private DateTime _countdownEnd;
#endregion

#region Timer
        [SerializeField]
        private int _fightTimeSeconds = 120;

        [SerializeField]
        private Text _timerText;

        private DateTime _timerStart;
#endregion

        [SerializeField]
        [ReadOnly]
        private int _actualFightTimeSeconds;

        [SerializeField]
        [ReadOnly]
        private bool _isRoundOver;

        [SerializeField]
        private FighterSpawn[] _spawnPoints;

#region Unity Lifecycle
        protected override void Start()
        {
            base.Start();

            _countdownText.text = _countdownSeconds.ToString();
            _timerText.text = _fightTimeSeconds.ToString();

            FighterManager.Instance.InitFighters(_spawnPoints);

            StartCountdown();
        }

        protected override void OnDestroy()
        {
            if(FighterManager.HasInstance) {
                FighterManager.Instance.DestroyFighters();
            }
        }

        private void Update()
        {
            if(_isRoundOver) {
                return;
            }

            if(_countdownContainer.activeInHierarchy) {
                UpdateCountdown();
            } else {
                UpdateTimer();
            }
        }
#endregion

        private void EndRound()
        {
            GameStageManager.Instance.LoadLobby();
        }

#region Countdown
        private void StartCountdown()
        {
            // plus 1 because frames
            _countdownEnd = DateTime.Now.AddSeconds(_countdownSeconds + 1);
        }

        private void UpdateCountdown()
        {
            TimeSpan countdownRemaining = _countdownEnd - DateTime.Now;
            if(countdownRemaining.TotalSeconds <= 0) {
                _countdownContainer.SetActive(false);
                StartTimer();
                return;
            }

            _countdownText.text = ((int)countdownRemaining.TotalSeconds).ToString();
        }
#endregion

#region Timer
        private void StartTimer()
        {
            _timerStart = DateTime.Now;
            _actualFightTimeSeconds = _fightTimeSeconds;
        }

        private void UpdateTimer()
        {
            DateTime timerEnd = _timerStart.AddSeconds(_actualFightTimeSeconds);
            TimeSpan timerRemaining = timerEnd - DateTime.Now;
            if(timerRemaining.TotalSeconds <= 0) {
                _timerText.text = "0";
                EndRound();
                return;
            }

            _timerText.text = ((int)timerRemaining.TotalSeconds).ToString();
        }
#endregion

#region Event Handlers
        protected override void MessageEventHandler(object sender, MessageEventArgs evt)
        {
            switch(evt.Message.type)
            {
            default:
                Debug.LogWarning($"Ignoring unexpected message type {evt.Message.type} from {evt.From}");
                break;
            }
        }
#endregion
    }
}
