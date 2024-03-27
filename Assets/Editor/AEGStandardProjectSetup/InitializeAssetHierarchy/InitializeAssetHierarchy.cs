using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AdmiralEgg.Tools
{
    public class InitializeAssetHierarchy
    {
        // Settings
        const string PROJECT_NAME = "DefaultProjectName";
        const string COMPANY_NAME = "DefaultCompanyName";

        // Add a _Project root
        static string[] assetHierarchyFolders = new string[]
        {
            "Docs",
            "Working",
            "ThirdParty",

            // All Project folders
            "_Project",
            "_Project/Animation",
            "_Project/Audio",
            "_Project/Data",
            "_Project/Materials",
            "_Project/Mesh",
            "_Project/Prefabs",
            "_Project/Scenes",
            "_Project/Scripts",
            "_Project/Scripts/Camera",
            "_Project/Scripts/Input",
            "_Project/Scripts/Managers",
            "_Project/Scripts/ScriptableObjects",
            "_Project/Scripts/UI",
            "_Project/Settings",
            "_Project/Shaders",
            "_Project/Sprites",
            "_Project/TexturesAndUV",
            "_Project/UI"
        };


        [MenuItem("Tools/Initial Setup Tools/[3] Initialize Asset Hierarchy", false, 2)]
        public static void Initialize()
        {
            // TODO: Don't run if it's already run? Create a setup log and check for that?
            CreateFolders();
            ConfigureProject();
            ConfigureEditor();

            // Refresh all
            AssetDatabase.Refresh();
        }

        static void ConfigureProject()
        {
            PlayerSettings.companyName = COMPANY_NAME;
            PlayerSettings.productName = PROJECT_NAME;
        }

        static void ConfigureEditor()
        {
            // Disable editor camera easing and acceleration
            SceneView.lastActiveSceneView.cameraSettings.easingEnabled = false;
            SceneView.lastActiveSceneView.cameraSettings.accelerationEnabled = false;

            // GIZMOS
            // Set sceneview gizmos size https://github.com/unity3d-kr/GizmoHotkeys/blob/05516ebfc3ce1655cbefb150d328e2b66e03646d/Editor/SelectionGizmo.cs
            Assembly asm = Assembly.GetAssembly(typeof(Editor));
            Type type = asm.GetType("UnityEditor.AnnotationUtility");
            if (type != null)
            {
                PropertyInfo iconSizeProperty = type.GetProperty("iconSize", BindingFlags.Static | BindingFlags.NonPublic);
                if (iconSizeProperty != null)
                {
                    iconSizeProperty.SetValue(asm, 0.01f, null);
                }
            }
        }

        /// <summary>
        /// Create folders and migrate scene to the new folder structure.
        /// </summary>
        static void CreateFolders()
        {
            string assetsFolder = Application.dataPath;

            // Create a folder if it doesn't exist.
            foreach (string folder in assetHierarchyFolders)
            {
                if (Directory.Exists($"{assetsFolder}/{folder}") == true) continue;

                Directory.CreateDirectory($"{assetsFolder}/{folder}");
            }
        }
    }
}
