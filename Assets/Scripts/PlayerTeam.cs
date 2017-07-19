using CatFight.AirConsole;

namespace CatFight
{
    public sealed class PlayerTeam
    {
        public enum TeamIds
        {
            TeamA = 1,
            TeamB = 2
        }

        private TeamIds _id = TeamIds.TeamA;

        public TeamIds Id
        {
            get { return _id; }

            set
            {
                _id = value;
                //AirConsoleManager.Instance.Message(_player.DeviceId, TODO: send team id message (and the controller would then set that in its custom state);
            }
        }

        private Player _player;

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
