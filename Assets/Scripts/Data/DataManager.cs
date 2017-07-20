using CatFight.Util;

using UnityEngine;

namespace CatFight.Data
{
    public sealed class DataManager : SingletonBehavior<DataManager>
    {
        [SerializeField]
        private TextAsset _gameDataFile;

        public TextAsset GameDataFile => _gameDataFile;

        public GameData GameData { get; private set; }

        public bool Load()
        {
            Debug.Log("Loading game data...");
            //Debug.Log(GameDataFile.text);

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
