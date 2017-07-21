using System.ComponentModel;

using CatFight.AirConsole;
using CatFight.AirConsole.Messages;

namespace CatFight
{
    public sealed class PlayerTeam
    {
        public enum TeamIds
        {
            [Description("None")]
            None = 0,

            [Description("Team A")]
            TeamA = 1,

            [Description("Team B")]
            TeamB = 2
        }

        private TeamIds _id = TeamIds.TeamA;

        public TeamIds Id
        {
            get { return _id; }

            set
            {
                _id = value;
                AirConsoleManager.Instance.Message(_player.DeviceId, new SetTeamMessage(_id));
            }
        }

        private readonly Player _player;

        public PlayerTeam(Player player)
        {
            _player = player;
        }

        public override string ToString()
        {
            return $"{Id}";
        }
    }
}
