using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AdmiralEgg.Tools
{
    public static class ConfigureDefaultSpinningCube
    {
        const string SCRIPTS_PATH = "Assets/_Project/Scripts";
        const string PACKAGE_PATH = "Assets/Editor/AEGStandardProjectSetup/AddSpinningCube";

        [MenuItem("Tools/Initial Setup Tools/[5] Add Spinning Cube", false, 4)]
        public static void Initialize()
        {
            // Copy scripts back to their original locations so we avoid assembly reference issues.
            CopyScript("SpinCube", SCRIPTS_PATH);
            CopyScript("PressEscapeToExit", SCRIPTS_PATH);

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "SpinCube";
            cube.transform.position += Vector3.forward * 3;

            EditorUtility.DisplayDialog("Manual Setup Step Required", "SpinCube created!\n\nDrag and drop scripts,\n- SpinCube\n- PressEscapeToExit\n\nFrom the _Project/Scripts folder onto the SpinCube.", "Finished");
        }

        private static void CopyScript(string scriptName, string copyPath)
        {
            string scriptGUID = AssetDatabase.FindAssets($"{scriptName} t:Script").FirstOrDefault();

            AssetDatabase.CopyAsset(AssetDatabase.GUIDToAssetPath(scriptGUID), Path.Join(copyPath, $"{scriptName}.cs"));
            AssetDatabase.Refresh();
        }
    }
}