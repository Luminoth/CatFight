using NDream.AirConsole;
using Newtonsoft.Json.Linq;

using UnityEngine;

using CatFight.Util;
using UnityEngine.SceneManagement;

namespace CatFight
{
    [RequireComponent(typeof(AirConsole))]
    public sealed class AirConsoleController : SingletonBehavior<AirConsoleController>
    {
        private void Start()
        {
            AirConsole.instance.onMessage += OnMessage;

            SceneManager.LoadSceneAsync("lobby", LoadSceneMode.Additive);
        }

        protected override void OnDestroy()
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
