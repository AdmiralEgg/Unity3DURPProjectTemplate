using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace AdmiralEgg.Tools
{
    public class UpdatePackageManifest
    {
        [MenuItem("Tools/Initial Setup Tools/[1] Update Package Manifest", false, 0)]
        public static void Initialize()
        {
            string manifestSearchString = "manifest_aeg3dprojectbase_0.1.0";

            // Get the path of the script templates for the active UnityEditor
            string packageManifestPath = Path.GetFullPath("Packages");

            string folderPath = GetEditorVersionFolderPath();

            if (AssetDatabase.IsValidFolder(folderPath) == false)
            {
                UnityEngine.Debug.LogWarning($"Found manifest folder path {folderPath} invalid. Package manifest will not update");
                return;
            }

            string manifestAssetPath = GetFullAssetPath( manifestSearchString, searchFolder: folderPath );

            UnityEngine.Debug.Log($"Full path: {manifestAssetPath}");

            CopyFile( manifestAssetPath, Path.Join(packageManifestPath, "manifest.json" ));

            // Refresh packages
            Client.Resolve();
        }

        private static void CopyFile(string templateFilePath, string defaultTemplatePath, bool withAdmin = false)
        {
            // Run a copy as admin
            var psi = new ProcessStartInfo();

            UnityEngine.Debug.Log($"Copying default script file: [{templateFilePath}] to [{defaultTemplatePath}].");

            psi.FileName = "cmd.exe";
            psi.Arguments = $"/c copy \"{templateFilePath}\" \"{defaultTemplatePath}\"";
            
            if (withAdmin == true)
            {
                psi.Verb = "runas";
            }

            var process = new Process();
            process.StartInfo = psi;
            process.Start();
            process.WaitForExit();

            AssetDatabase.Refresh();
        }

        private static string GetEditorVersionFolderPath()
        {
            int lastDotIndex = Application.unityVersion.LastIndexOf('.');

            // Remove characters after and including the last "."
            string resultString = Application.unityVersion.Substring(0, lastDotIndex);

            string folderguid = AssetDatabase.FindAssets($"{resultString}, t:Folder").FirstOrDefault();
            string folderPath = AssetDatabase.GUIDToAssetPath(folderguid);
            
            return folderPath;
        }

        private static string GetFullAssetPath(string searchString, string searchFolder = null, string type = "TextAsset")
        {
            AssetDatabase.Refresh();

            // Find the default script in the Asset Database
            string guid = null;
            
            if (searchFolder == null)
            {
                guid = AssetDatabase.FindAssets($"{searchString}, t:{type}").FirstOrDefault();
            }
            else
            {
                guid = AssetDatabase.FindAssets($"{searchString}, t:{type}", new[] { searchFolder }).FirstOrDefault();
            }

            if (guid == null)
            {
                return null;
            }

            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            // Get parent Assets path
            string assetDatabaseParentPath = Directory.GetParent(Application.dataPath).ToString();

            string fullPath = Path.GetFullPath(Path.Join(assetDatabaseParentPath, assetPath));
            return fullPath;
        }
    }

    public class RevertPackageManifest
    {
        [MenuItem("Tools/Initial Setup Tools/Revert To Default Package Manifest", false, 200)]
        public static void Initialize()
        {
            Client.ResetToEditorDefaults();
        }
    }
}
