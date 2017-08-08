using System.IO;

using CatFight.Data;

using NDream.AirConsole;

using UnityEditor;
using UnityEngine;

namespace CatFight.Editor
{
    [InitializeOnLoad]
    public static class AssetProcessor
    {
#region Game Data
        // GameData asset resource location
        private const string GameDataResourcePath = "Data/GameData";

        private const string ControllerGameDataPath = "controller/controller/src/assets/data/GameData.json";
#endregion

#region Controller
        // this is the file that is attached to the AirConsole object in the main scene
        // we'll overwrite this with the index.html from the Angular project
        private const string ControllerAssetDestPath = "Assets/controller.html";

        private  const string ControllerAssetPath = "controller/controller";

        private const string ControllerDeployFilename = "deploy.bat";
#endregion

        [MenuItem("Cat Fight/Deploy Controller")]
        private static void DeployController()
        {
            WriteControllerGameData();

            DeployControllerAssets();
        }

        private static void WriteControllerGameData()
        {
            GameData gameData = Resources.Load<GameData>(GameDataResourcePath);

            Debug.Log($"Writing game data to {ControllerGameDataPath}...");
            File.WriteAllText(ControllerGameDataPath, gameData.ToJson());
        }

        private static void DeployControllerAssets()
        {
            string deployPath = Path.Combine(Directory.GetCurrentDirectory(), ControllerAssetPath).Replace('/', Path.DirectorySeparatorChar);
            string controllerAssetPath = Path.Combine(Directory.GetCurrentDirectory(), ControllerAssetDestPath).Replace('/', Path.DirectorySeparatorChar);
            string controllerAssetsPath = Path.Combine(Directory.GetCurrentDirectory(), $"Assets{Settings.WEBTEMPLATE_PATH}/").Replace('/', Path.DirectorySeparatorChar);

            Debug.Log($"Deploying controller to {controllerAssetsPath}...");
            //EditorUtility.DisplayDialog("Deploying Controller", $"Deploying controller to {controllerAssetsPath}, please wait...", "Ok");

            if(!Executor.ExecuteScript(deployPath, "cmd.exe",
                process => {
                    Debug.Log($"Controller deployment completed: {process.ExitCode}");
                    //EditorUtility.DisplayDialog("Deploy Complete", $"Controller deployment completed: {process.ExitCode}", "Ok");
                },
                $"/c {ControllerDeployFilename} \"{controllerAssetPath}\" \"{controllerAssetsPath}\""))
            {
                EditorUtility.DisplayDialog("Process Error", "Unable to start controller deploy process!", "Ok");
            }
        }
    }
}
