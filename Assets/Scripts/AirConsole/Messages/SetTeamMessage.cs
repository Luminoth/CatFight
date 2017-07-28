using System;

using CatFight.Players;
using CatFight.Util;

using Newtonsoft.Json.Linq;

namespace CatFight.AirConsole.Messages
{
// TODO: this should be a custom state variable for the screen
// and then controllers can figure out their team by looking through that state
    [Serializable]
    public sealed class SetTeamMessage : Message
    {
        public override MessageType type => MessageType.SetTeam;

        public int teamId { get; set; }

        public string teamName { get; set; }

        public SetTeamMessage(JToken data)
            : base(data)
        {
            teamId = (int)data["teamId"];
            teamName = (string)data["teamName"];
        }

        public SetTeamMessage(Player.TeamIds id)
        {
            teamId = (int)id;
            teamName = id.GetDescription();
        }

        private SetTeamMessage()
        {
        }

        public override string ToString()
        {
            return $"SetTeamMessage({teamId}: {teamName})";
        }
    }
}
