using System.Collections.Generic;

using CatFight.Players;
using CatFight.Util;

namespace CatFight.Fighters
{
    public sealed class FighterState
    {
        public Player.TeamIds teamId { get; }

        public float currentHealth { get; }

        private readonly Dictionary<string, int> _specialsRemaining = new Dictionary<string, int>();

        public IReadOnlyDictionary<string, int> specialsRemaining => _specialsRemaining;

        public FighterState(Fighter fighter)
        {
            teamId = fighter.TeamId;
            currentHealth = fighter.Stats.CurrentHealth;

            foreach(var kvp in fighter.Stats.Specials) {
                _specialsRemaining.Add(kvp.Key.ToString(), kvp.Value.RemainingUses);
            }
        }
    }
}
