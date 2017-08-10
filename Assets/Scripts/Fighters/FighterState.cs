using System.Collections.Generic;

using CatFight.Items.Specials;

namespace CatFight.Fighters
{
    public sealed class FighterState
    {
        public int teamId { get; }

        public float currentHealth { get; }

        private readonly Dictionary<string, int> _specialsRemaining = new Dictionary<string, int>();

        public IReadOnlyDictionary<string, int> specialsRemaining => _specialsRemaining;

        public FighterState(Fighter fighter)
        {
            teamId = fighter.Team.Id;
            currentHealth = fighter.Stats.CurrentHealth;

            foreach(Special special in fighter.Stats.Specials) {
                _specialsRemaining.Add(special.SpecialType.ToString(), special.RemainingUses);
            }
        }
    }
}
