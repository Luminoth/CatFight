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

        private static void CopyControllerResources()
        {
            DataManager dataManager = Object.FindObjectOfType<DataManager>();
            if(null == dataManager) {
                Debug.LogError("Missing DataManager!");
                return;
            }

            Debug.Log("Copying controller resources...");
            CopyGameData(dataManager);
        }

        private static void CopyGameData(DataManager dataManager)
        {
            string src = Path.Combine(Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(dataManager.GameDataFile));
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
                CopyControllerResources();
            } else if(_isPlaying && !EditorApplication.isPaused) {
                _isPlaying = false;
                CopyControllerResources();
            }
        }
#endregion
    }
}
