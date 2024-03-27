using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdmiralEgg.Tools
{
    public class BuildScene
    {
        public enum SceneHierachyHeaders { Managers, Camera, Lighting, Objects, Environment, UI }

        [MenuItem("Tools/Initial Setup Tools/[2] Build Default Scene", false, 1)]
        public static void Initialize()
        {
            const string SCENE_NAME = "NewDefaultScene";
            const string SCENE_PATH = "Assets/Scenes";

            Dictionary<SceneHierachyHeaders, GameObject> headerLookup = new Dictionary<SceneHierachyHeaders, GameObject>();

            // create new scene
            Scene scene = EditorSceneManager.NewScene(new NewSceneSetup());

            Array headers = Enum.GetValues(typeof(SceneHierachyHeaders));

            foreach (SceneHierachyHeaders headerName in headers)
            {
                GameObject go = new GameObject($"--- {headerName.ToString().ToUpper()} ---");
                headerLookup.Add(headerName, go);
            }
        
            // move main camera and directional light to game objects
            GameObject mainLight = new GameObject("MainLight");
            mainLight.transform.parent = headerLookup[SceneHierachyHeaders.Lighting].transform;

            Light light = mainLight.AddComponent<Light>();
            light.type = LightType.Directional;

            GameObject mainCamera = new GameObject("MainCamera");
            mainCamera.transform.parent = headerLookup[SceneHierachyHeaders.Camera].transform;

            Camera camera = mainCamera.AddComponent<Camera>();
            camera.tag = "MainCamera";

            // save the scene
            string scenePath = Path.Join($"{SCENE_PATH}", $"{SCENE_NAME}.unity");

            bool sceneSaved = EditorSceneManager.SaveScene(scene, scenePath);

            if (sceneSaved == false) 
            {
                Debug.LogError("Could not save scene.");
                return;
            }

            // add it at the top of the build
            string sceneAssetDBGUID = AssetDatabase.FindAssets(SCENE_NAME, new[] { SCENE_PATH }).FirstOrDefault();


            EditorBuildSettingsScene ebsc = new EditorBuildSettingsScene();
            ebsc.path = AssetDatabase.GUIDToAssetPath(sceneAssetDBGUID);
            ebsc.enabled = true;
        
            EditorBuildSettings.scenes = new[] { ebsc };
        }
    }
}