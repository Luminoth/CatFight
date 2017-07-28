﻿using System.Collections.Generic;

using CatFight.Fighters;

namespace CatFight.Stages.Arena
{
    public sealed class GameState
    {
        public bool isGameStarted { get; }

        private readonly Dictionary<int, FighterState> _fighterState = new Dictionary<int, FighterState>();

        public IReadOnlyDictionary<int, FighterState> fighterState => _fighterState;

        public GameState()
        {
            isGameStarted = GameStageManager.Instance.IsGameStarted;

            foreach(var kvp in FighterManager.Instance.Fighters) {
                _fighterState.Add((int)kvp.Key, new FighterState(kvp.Value));
            }
        }
    }
}
