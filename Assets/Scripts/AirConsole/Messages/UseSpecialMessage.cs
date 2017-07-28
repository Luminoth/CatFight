using System;

using CatFight.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CatFight.AirConsole.Messages
{
    [Serializable]
    public sealed class UseSpecialMessage : Message
    {
        public override MessageType type => MessageType.UseSpecial;

        private string _specialType;

        public string specialType
        {
            get { return _specialType; }

            set
            {
                SpecialData.SpecialType scratch;
                if(Enum.TryParse(value, out scratch)) {
                    _specialType = value;
                    SpecialType = scratch;
                }
            }
        }

        [JsonIgnore]
        public SpecialData.SpecialType SpecialType { get; private set; }

        public UseSpecialMessage(JToken data)
            : base(data)
        {
            specialType = (string)data["specialType"];
        }

        private UseSpecialMessage()
        {
        }

        public override string ToString()
        {
            return $"UseSpecialMessage({SpecialType})";
        }
    }
}
