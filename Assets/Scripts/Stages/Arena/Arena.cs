using System;
using System.Collections;

using CatFight.AirConsole;
using CatFight.AirConsole.Messages;
using CatFight.Data;
using CatFight.Fighters;
using CatFight.Players;
using CatFight.Util;

using UnityEngine;
using UnityEngine.UI;

namespace CatFight.Stages.Arena
{
    public sealed class Arena : Stage
    {
        private const string GameStateKey = "gameState";

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

// TODO: this needs to be fixed to account for more than 2 teams
#region Team A
        [SerializeField]
        private Text _teamAMissilesRemaining;

        [SerializeField]
        private Text _teamAChaffRemaining;
#endregion

#region Team B
        [SerializeField]
        private Text _teamBMissilesRemaining;

        [SerializeField]
        private Text _teamBChaffRemaining;
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

            StartCoroutine(UpdateControllers());

            StartCountdown();
        }

        protected override void OnDestroy()
        {
            StopAllCoroutines();

            if(FighterManager.HasInstance) {
                FighterManager.Instance.Cleanup();
            }

            base.OnDestroy();
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

            UpdateFighters();
        }
#endregion

        private IEnumerator UpdateControllers()
        {
            while(true) {
                yield return new WaitForSeconds(1.0f);

                AirConsoleManager.Instance.SetCustomDeviceStateProperty(GameStateKey, new GameState());
            }
        }

        private void UpdateFighters()
        {
            // TODO: this needs to be updated to account for more than 2 teams
            // basically just need the UI to be fixed is all
            Fighter fighterA = FighterManager.Instance.GetFighter(1);
            if(null != fighterA) {
                _teamAMissilesRemaining.text = fighterA.Stats.GetSpecialRemaining(SpecialData.SpecialType.Missiles).ToString();
                _teamAChaffRemaining.text = fighterA.Stats.GetSpecialRemaining(SpecialData.SpecialType.Chaff).ToString();
                if(fighterA.Stats.IsDead) {
                    EndRound();
                }
            }

            Fighter fighterB = FighterManager.Instance.GetFighter(2);
            if(null != fighterB) {
                _teamBMissilesRemaining.text = fighterB.Stats.GetSpecialRemaining(SpecialData.SpecialType.Missiles).ToString();
                _teamBChaffRemaining.text = fighterB.Stats.GetSpecialRemaining(SpecialData.SpecialType.Chaff).ToString();
                if(fighterB.Stats.IsDead) {
                    EndRound();
                }
            }
        }

        private void EndRound()
        {
            _isRoundOver = true;
            GameStageManager.Instance.IsGameStarted = false;

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
            GameStageManager.Instance.IsGameStarted = true;

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

        private void UseSpecial(int deviceId, SpecialData.SpecialType specialType)
        {
            Player player = PlayerManager.Instance.GetPlayer(deviceId);
            if(null == player) {
                Debug.LogError($"No such player {deviceId}!");
                return;
            }

            Fighter fighter = FighterManager.Instance.GetFighter(player.Team.Id);
            if(null == fighter) {
                Debug.LogError($"No fighter for team {player.Team.Id}!");
                return;
            }
            fighter.Stats.UseSpecial(specialType);
        }

#region Event Handlers
        protected override void MessageEventHandler(object sender, MessageEventArgs evt)
        {
            switch(evt.Message.type)
            {
            case Message.MessageType.UseSpecial:
                UseSpecialMessage useSpecialMessage = (UseSpecialMessage)evt.Message;
                UseSpecial(evt.From, useSpecialMessage.SpecialType);
                break;
            default:
                Debug.LogWarning($"Ignoring unexpected message type {evt.Message.type} from {evt.From}");
                break;
            }
        }
#endregion
    }
}
