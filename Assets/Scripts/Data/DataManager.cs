using CatFight.Util;

using UnityEngine;

namespace CatFight.Data
{
    public sealed class DataManager : SingletonBehavior<DataManager>
    {
        [SerializeField]
        private GameData _gameData;

        public GameData GameData => _gameData;

#region Unity Lifecycle
        private void Start()
        {
            GameData.Initialize();
            GameData.DebugDump();
        }
#endregion
    }
}
