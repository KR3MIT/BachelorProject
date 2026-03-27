using System;
using UnityEngine;
using UnityEditor;

public class AssetBundle
{
    [MenuItem("Assets/Build AssetBundles")]
    private static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = Application.dataPath + "/AssetBundle";
        try
        {
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to create AssetBundles directory: {e.Message}");
        }
    }
}
