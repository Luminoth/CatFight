using CatFight.Util;

using UnityEngine;

namespace CatFight.Data
{
    public sealed class DataManager : SingletonBehavior<DataManager>
    {
        [SerializeField]
        private TextAsset _gameDataFile;

        public TextAsset GameDataFile => _gameDataFile;

        [SerializeField]
        [ReadOnly]
        private GameData _gameData;

        public GameData GameData { get { return _gameData; } private set { _gameData = value; } }

        public bool Load()
        {
            Debug.Log("Loading game data...");

// TODO: show an error dialog
            GameData = JsonUtility.FromJson<GameData>(GameDataFile.text);
            if(null == GameData) {
                Debug.LogError("There was an error loading the game data!");
                return false;
            }

            if(!GameData.IsValid) {
                Debug.LogError($"Invalid game data version. Got {GameData.Version}, expected {GameData.CurrentVersion}!");
                return false;
            }

            GameData.Process();
            GameData.DebugDump();

            return true;
        }
    }
}
