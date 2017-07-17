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
        public static void CopyControllerResources(BuildTarget target, string pathToBuiltProject)
        {
            CopyControllerResources();
        }

        private static void CopyControllerResources()
        {
            AirConsoleController controller = Object.FindObjectOfType<AirConsoleController>();
            if(null == controller) {
                Debug.LogError("Missing AirConsole controller!");
                return;
            }

            Debug.Log("Copying controller resources...");
            CopyGameData(controller);
        }

        private static void CopyGameData(AirConsoleController controller)
        {
            string src = Path.Combine(Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(controller.GameDataFile));
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
