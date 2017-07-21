using System;

using CatFight.Players;
using CatFight.Util;

using Newtonsoft.Json.Linq;

namespace CatFight.AirConsole.Messages
{
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

        public SetTeamMessage(PlayerTeam.TeamIds id)
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
