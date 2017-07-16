using NDream.AirConsole;
using Newtonsoft.Json.Linq;

using UnityEngine;

using CatFight.Util;

namespace CatFight
{
    [RequireComponent(typeof(AirConsole))]
    public class AirConsoleController : MonoBehavior
    {
        private void Start()
        {
            AirConsole.instance.onMessage += OnMessage;
        }

        private void OnDestroy()
        {
            AirConsole.instance.onMessage -= OnMessage;
        }

        private void OnMessage(int from, JToken data)
        {
            Debug.Log($"OnMessage({from}: {data})");

            AirConsole.instance.Message(from, "Hello World!");
        }
    }
}
