using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Linq;
using System;

namespace AdmiralEgg.Tools.URP
{
    /// <summary>
    /// Setup configuration assets.
    /// Create and adjust gameobjects.
    /// If asset folders or gameobjects can't be found, add to root.
    /// </summary>
    public class ConfigureURP
    {
        static string settingsPath = "Assets/_Project/Settings";
        static string materialsPath = "Assets/_Project/Materials";
        
        // Asset Names
        const string URD_NAME = "DefaultURPRenderData.asset";
        const string URPA_NAME = "DefaultURPRenderPipelineAsset.asset";
        const string DEFAULT_URP_GLOBALSETTINGS_NAME = "UniversalRenderPipelineGlobalSettings.asset";
        const string LIGHTING_NAME = "DefaultLightingSettings.asset";
        const string SKYBOXMATERIAL_NAME = "DefaultSkybox.mat";

        [MenuItem("Tools/Initial Setup Tools/[4] Configure URP Rendering", false, 3)]
        public static void Initialize()
       {
            // Initial refresh to make sure we're working with current asset data
            AssetDatabase.Refresh();

            CheckAssetPath(settingsPath);
            CheckAssetPath(materialsPath);

            UniversalRendererData urd = ConfigureRendererData(); // This is derived from ScriptableRendererData.

            UniversalRenderPipelineAsset urpa = null;

            if (urd != null)
            {
                urpa = ConfigureRenderPipelineAsset(urd); // Derived from RenderPipelineAsset
            }

            // Sets the default pipeline and creates a Global Settings asset in 'Assets'
            if (urpa != null)
            {
                GraphicsSettings.defaultRenderPipeline = urpa;
            }

            // Refresh database so we can apply new pipeline assets to the GraphicsSettings
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.ForceReserializeAssets();

            ConfigureRenderPipelineGlobalSettings();
            LightingSettings ls = ConfigureLightingSettings();

            // Get the new lighting settings asset and apply to current scene.
            if (ls != null)
            {
                Lightmapping.lightingSettings = ls;
            }

            ConfigureSkyboxMaterial();
        }

        private static void CheckAssetPath(string path)
        {    
            string assetDatabaseParentPath = Directory.GetParent(Application.dataPath).ToString();

            string fullPath = Path.GetFullPath(Path.Join(assetDatabaseParentPath, path));

            Debug.Log($"Path to check: {fullPath}");

            // if a path can't be found, initialise at root.
        }

        private static bool AssetExistsInAssetDatabase(string assetName)
        {
            if (AssetDatabase.FindAssets(Path.GetFileNameWithoutExtension(assetName)).FirstOrDefault() != null)
            {
                Debug.Log($"Asset {URD_NAME} already exists, skipping creation.");
                return true;
            };

            return false;
        }

        private static UniversalRendererData ConfigureRendererData()
        {
            if (AssetExistsInAssetDatabase(URD_NAME)) return null;

            var urdAsset = ScriptableObject.CreateInstance<UniversalRendererData>();

            // Configure default settings
            urdAsset.renderingMode = RenderingMode.ForwardPlus;

            // TODO: Review whether post processing needs to be turned on.
            // TODO: one URPData per platform?

            AssetDatabase.CreateAsset(urdAsset, Path.Combine(settingsPath, URD_NAME));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return urdAsset;
        }

        private static UniversalRenderPipelineAsset ConfigureRenderPipelineAsset(UniversalRendererData urd)
        {
            if (AssetExistsInAssetDatabase(URPA_NAME)) return null;

             UniversalRenderPipelineAsset urpaAsset = UniversalRenderPipelineAsset.Create(urd);

            // Configure default settings
            urpaAsset.supportsHDR = true;
            urpaAsset.msaaSampleCount = 4;

            AssetDatabase.CreateAsset(urpaAsset, Path.Combine(settingsPath, URPA_NAME));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return urpaAsset;
        }

        /// <summary>
        /// Configures the RenderPipelineGlobalSettings asset and moves it to the Settings folder.
        /// </summary>
        private static void ConfigureRenderPipelineGlobalSettings()
        {
            AssetDatabase.Refresh();

            var urpGlobalSettingsGUID = AssetDatabase.FindAssets(Path.GetFileNameWithoutExtension(DEFAULT_URP_GLOBALSETTINGS_NAME)).FirstOrDefault();

            if (urpGlobalSettingsGUID == null)
            {
                Debug.LogWarning($"Cannot find [{Path.GetFileNameWithoutExtension(DEFAULT_URP_GLOBALSETTINGS_NAME)}] in the Assets folder. Nothing to move, so skipping.");
                return;
            }
            else
            {
                string currentPath = AssetDatabase.GUIDToAssetPath(urpGlobalSettingsGUID);
                AssetDatabase.MoveAsset(currentPath, Path.Join(settingsPath, DEFAULT_URP_GLOBALSETTINGS_NAME));
            }

            // Configuration

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static LightingSettings ConfigureLightingSettings()
        {
            if (AssetExistsInAssetDatabase(LIGHTING_NAME)) return null;

            LightingSettings ls = new LightingSettings();

            // Configure Default Settings
            ls.realtimeGI = true;
            ls.realtimeEnvironmentLighting = true;
            ls.indirectResolution = 2;
            
            // Baked settings (off by default)
            ls.bakedGI = false;
            ls.lightmapper = LightingSettings.Lightmapper.ProgressiveGPU;
            ls.lightmapResolution = 40;

            AssetDatabase.CreateAsset(ls, Path.Combine(settingsPath, $"{LIGHTING_NAME}"));

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return ls;
        }

        private static void ConfigureSkyboxMaterial()
        {
            if (AssetExistsInAssetDatabase(SKYBOXMATERIAL_NAME)) return;

            var skyboxShader = Shader.Find(("Skybox/Procedural"));

            // Configure the skybox shader attributes

            Material skyboxMaterial = new Material(skyboxShader);

            AssetDatabase.CreateAsset(skyboxMaterial, Path.Combine(materialsPath, SKYBOXMATERIAL_NAME));
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            RenderSettings.skybox = skyboxMaterial;
        }
    }
}