using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO.Compression;
using System;

namespace AdmiralEgg.Tools
{
    public class ConfigureGitHubSetup
    {
        [MenuItem("Tools/Initial Setup Tools/Add .gitignore and GitHub Actions To Project Root", false, 300)]
        public static void Initialize()
        {
            const string GITIGNORE_SEARCHSTRING = "gitignore";
            const string GITIGNORE_NAME = ".gitignore";
            const string GITACTIONSDIR_SEARCHSTRING = "githubactions";
            const string GITACTIONSDIR_NAME = ".github";
            const string VERSION = "0_1_0";

            AssetDatabase.Refresh();

            // copy gitignore and github actions to root of project.
            string gitIgnoreGUID = AssetDatabase.FindAssets(GITIGNORE_SEARCHSTRING, new[] { $"Assets/Editor/AEGStandardProjectSetup/ConfigureGitSetup/{VERSION}" }).FirstOrDefault();
            string gitActionsGUID = AssetDatabase.FindAssets(GITACTIONSDIR_SEARCHSTRING, new[] { $"Assets/Editor/AEGStandardProjectSetup/ConfigureGitSetup/{VERSION}" }).FirstOrDefault();

            if (gitIgnoreGUID != null) 
            {
                Debug.Log("GitIgnore:" + AssetDatabase.GUIDToAssetPath(gitIgnoreGUID));
            }

            if (gitActionsGUID != null)
            {
                Debug.Log("GitHub:" + AssetDatabase.GUIDToAssetPath(gitActionsGUID));
            }

            string assetDatabaseParentPath = Directory.GetParent(Application.dataPath).ToString();

            string fullPathGitIgnore = Path.GetFullPath(Path.Join(assetDatabaseParentPath, AssetDatabase.GUIDToAssetPath(gitIgnoreGUID)));
            string fullPathGitActions = Path.GetFullPath(Path.Join(assetDatabaseParentPath, AssetDatabase.GUIDToAssetPath(gitActionsGUID)));

            try
            {
                ZipFile.CreateFromDirectory(fullPathGitActions, @".\githubactions.zip");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Could not create zip file: {ex}");
            }

            
            //try
            //{
            //    ZipFile.ExtractToDirectory(@".\githubactions.zip", GITACTIONSDIR_NAME);
            //}
            //catch (Exception ex)
            //{
            //    Debug.LogWarning($"Could not extract zip file: {ex}");
            //}

            //// Cleanup .meta files
            //if (File.Exists(@".\githubactions.zip"))
            //{
            //    File.Delete(@".\githubactions.zip");
            //}

            try
            {
                File.Copy(fullPathGitIgnore, Path.Join($"{Directory.GetParent(Application.dataPath)}", GITIGNORE_NAME), false);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Could not copy file: {ex}");
            }
        }
    }
}