using System.Collections.Generic;

namespace CatFight.Players
{
    public sealed class Team
    {
        public int TeamId { get; }

        private readonly List<Player> _players = new List<Player>();

        public IReadOnlyCollection<Player> Players => _players;

        public Team(int teamId)
        {
            TeamId = teamId;
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }
    }
}
