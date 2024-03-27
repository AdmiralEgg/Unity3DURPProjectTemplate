using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AdmiralEgg.Tools
{
    /// <summary>
    /// Copies a local DefaultScript.cs and overwrites the standard Unity script.
    /// This updates local UnityEditor files, which is not part of the project files.
    /// </summary>
    public class ConfigureScriptTemplate
    {
        [MenuItem("Tools/Initial Setup Tools/Update Script Template With Local File (Admin Required)", false, 100)]
        public static void UpdateScriptTemplateWithLocalFile()
        {
            string defaultScriptTemplateSearchString = "NewBehaviourScript";

            // Get the path of the script templates for the active UnityEditor
            string defaultTemplatePath = Path.Combine(EditorApplication.applicationContentsPath, "Resources\\ScriptTemplates");

            // Get the path to the script template
            string fullPath = GetFullAssetPath(defaultScriptTemplateSearchString);

            if (fullPath == null)
            {
                UnityEngine.Debug.LogError($"Could not find text file in AssetDatabase using search string: [{defaultScriptTemplateSearchString}]. Exiting.");
                return;
            }

            CopyFile(fullPath, defaultTemplatePath, withAdmin: true);
        }

        private static string GetFullAssetPath(string searchString, string type = "TextAsset")
        {
            AssetDatabase.Refresh();

            // Find the default script in the Asset Database
            string guid = AssetDatabase.FindAssets($"{searchString}, t:{type}").FirstOrDefault();

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
    }
}