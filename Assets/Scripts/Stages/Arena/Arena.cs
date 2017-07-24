using System;
using System.Collections.Generic;

using CatFight.AirConsole;
using CatFight.Fighters;
using CatFight.Util;

using CatFight.AirConsole.Messages;

using UnityEngine;
using UnityEngine.UI;
using CatFight.Players;

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

        [SerializeField]
        private Fighter _fighterPrefab;

        [SerializeField]
        [ReadOnly]
        private GameObject _fighterContainer;

        private readonly List<Fighter> _fighters = new List<Fighter>();

#region Unity Lifecycle
        protected override void Start()
        {
            base.Start();
            if (_countdownContainer != null)
            {
                _countdownText.text = _countdownSeconds.ToString();
            }

            if (_timerText != null)
            {
                _timerText.text = _fightTimeSeconds.ToString();
            }

            _fighterContainer = new GameObject("Fighters");
            InitFighters();

            StartCountdown();
        }

        protected override void OnDestroy()
        {
            Destroy(_fighterContainer);
            _fighterContainer = null;

            base.OnDestroy();
        }

        private void Update()
        {
            if(_isRoundOver) {
                return;
            }

            if (_countdownContainer != null)
            {
                if (_countdownContainer.activeInHierarchy)
                {
                    UpdateCountdown();
                }
                else
                {
                    UpdateTimer();
                }
            }
        }
#endregion

        private void EndRound()
        {
            GameStageManager.Instance.LoadLobby();
        }

#region Fighters
        private void InitFighters()
        {
            // TODO: clear out the _fighterContainer children

            foreach(FighterSpawn spawnPoint in _spawnPoints) {
                Fighter fighter = SpawnFighter(spawnPoint);
                _fighters.Add(fighter);

                fighter.Initialize(spawnPoint.TeamId);
            }
        }

        private Fighter SpawnFighter(FighterSpawn spawnPoint)
        {
            Fighter fighter = Instantiate(_fighterPrefab, _fighterContainer.transform);
            fighter.transform.position = spawnPoint.transform.position;
            return fighter;
        }
#endregion

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
            case Message.MessageType.ControllerAction:
                SetInputMessage setSlotMessage = (SetInputMessage)evt.Message;

                    Player player;
                    if (!PlayerManager.Instance.Players.TryGetValue(evt.From, out player))
                    {
                        Debug.LogError($"Player {evt.From} not found.");
                        return;
                    }

                    //Need to convert data to a betting input class
                    for (int i = 0; _fighters.Count > i ; i++)
                    {
                        if(_fighters[i].TeamId == player.Team.Id)
                        {
                            _fighters[i].Input(setSlotMessage);
                            break;
                        }
                    }

                    break;
            default:
                Debug.LogWarning($"Ignoring unexpected message type {evt.Message.type} from {evt.From}");
                break;
            }
        }
#endregion
    }
}
