using System;

using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CatFight.AirConsole.Messages
{
    [Serializable]
    public sealed class SetInputMessage : Message
    {
        public override MessageType type => MessageType.ControllerAction;

        public inputButtons inputButton { get; set; }
        public int fireType { get; set; }
        public bool buttonState { get; set; }

        public SetInputMessage(JToken data)
            : base(data)
        {
            string button = (string)data["button"];

            switch (button) {
                case "up":
                    inputButton = inputButtons.up;
                    break;
                case "down":
                    inputButton = inputButtons.down;
                    break;
                case "left":
                    inputButton = inputButtons.left;
                    break;
                case "right":
                    inputButton = inputButtons.right;
                    break;
                case "fire":
                    inputButton = inputButtons.fire;
                    break;
                default:
                    Debug.LogError($"Unsupported input type: {button}");
                    break;
            }

            fireType = (int)data["fireType"];
            buttonState = (string)data["buttonState"] == "down" ? true : false;
        }

        public SetInputMessage()
        {
        }

        public override string ToString()
        {
            return $"SetInputMessage({inputButton}: {fireType}: {buttonState})";
        }


        public enum inputButtons
        {
            up,
            down,
            left,
            right,
            fire
        }

    }
}