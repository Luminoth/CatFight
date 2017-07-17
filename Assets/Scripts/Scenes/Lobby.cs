﻿using CatFight.AirConsole;
using CatFight.AirConsole.Messages;
using CatFight.Util;

namespace CatFight.Scenes
{
    public sealed class Lobby : SingletonBehavior<Lobby>
    {
        private void Start()
        {
            AirConsoleController.Instance.MessageEvent += MessageEventHandler;
        }

        protected override void OnDestroy()
        {
            if(AirConsoleController.HasInstance) {
                AirConsoleController.Instance.MessageEvent -= MessageEventHandler;
            }
        }

        private void MessageEventHandler(object sender, MessageEvent evt)
        {
            AirConsoleController.Instance.Message(evt.From, new DebugMessage
                {
                    message = $"Hello World {evt.From}"
                }
            );
        }
    }
}
