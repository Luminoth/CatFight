using System.IO;

using CatFight.Data;

using NDream.AirConsole;

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CatFight.Editor
{
    [InitializeOnLoad]
    public static class AssetProcessor
    {
        private const string GameDataResourcePath = "Data/GameData";

        private static bool _isPlaying;

    	static AssetProcessor()
        {
            _isPlaying = !EditorApplication.isPlayingOrWillChangePlaymode;
            EditorApplication.playmodeStateChanged += OnUnityPlayModeChanged;
		}

        [PostProcessBuild]
        public static void CopyControllerResources(BuildTarget target, string pathToBuiltProject)
        {
            CopyControllerResources();
        }

        [MenuItem("Cat Fight/Data/Copy Controller Resources")]
        private static void CopyControllerResources()
        {
            string dest = Path.Combine(Directory.GetCurrentDirectory(), "Assets" + Settings.WEBTEMPLATE_PATH + "/data/GameData.json");
            GameData gameData = Resources.Load<GameData>(GameDataResourcePath);

            Debug.Log($"Copying game data to {dest}...");
            File.WriteAllText(dest, gameData.ToJson());
        }

#region Event Handlers
        private static void OnUnityPlayModeChanged()
        {
            // do things either when starting to play or finishing play
            if(!_isPlaying && EditorApplication.isPlayingOrWillChangePlaymode) {
                _isPlaying = true;
                CopyControllerResources();
            } else if(_isPlaying && !EditorApplication.isPaused) {
                _isPlaying = false;
                CopyControllerResources();
            }
        }
#endregion
    }
}
