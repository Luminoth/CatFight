using System.IO;

using CatFight.AirConsole;

using NDream.AirConsole;

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CatFight.Editor
{
    [InitializeOnLoad]
    public static class AssetProcessor
    {
        private static bool _isPlaying;

    	static AssetProcessor()
        {
            _isPlaying = !EditorApplication.isPlayingOrWillChangePlaymode;
            EditorApplication.playmodeStateChanged += OnUnityPlayModeChanged;
		}

        [PostProcessBuild]
        public static void CopyAirConsoleResources(BuildTarget target, string pathToBuiltProject)
        {
            CopyAirConsoleResources();
        }

        private static void CopyAirConsoleResources()
        {
            AirConsoleManager airConsoleManager = Object.FindObjectOfType<AirConsoleManager>();
            if(null == airConsoleManager) {
                Debug.LogError("Missing AirConsoleManager!");
                return;
            }

            Debug.Log("Copying AirConsole resources...");
            CopyGameData(airConsoleManager);
        }

        private static void CopyGameData(AirConsoleManager airConsoleManager)
        {
            string src = Path.Combine(Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(airConsoleManager.GameDataFile));
            string dest = Path.Combine(Directory.GetCurrentDirectory(), "Assets" + Settings.WEBTEMPLATE_PATH + "/data/GameData.json");

            Debug.Log($"Copying game data from {src} to {dest}...");
            File.Copy(src, dest, true);
        }

#region Event Handlers
        private static void OnUnityPlayModeChanged()
        {
            // do things either when starting to play or finishing play
            if(!_isPlaying && EditorApplication.isPlayingOrWillChangePlaymode) {
                _isPlaying = true;
                CopyAirConsoleResources();
            } else if(_isPlaying && !EditorApplication.isPaused) {
                _isPlaying = false;
                CopyAirConsoleResources();
            }
        }
#endregion
    }
}
