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

#region Unity Lifecycle
        private void Start()
        {
            if(!LoadGameData()) {
                return;
            }
        }
#endregion

        private bool LoadGameData()
        {
            Debug.Log("Loading game data...");
            //Debug.Log(GameDataFile.text);

// TODO: show an error dialog
            GameData = JsonUtility.FromJson<GameData>(GameDataFile.text);
            if(null == GameData) {
                Debug.LogError("There was an error loading the game data!");
                return false;
            }

            GameData.Process();
            GameData.DebugDump();

            return true;
        }
    }
}
