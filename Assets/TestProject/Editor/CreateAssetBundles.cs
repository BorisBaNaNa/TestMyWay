using UnityEditor;

public class CreateAssetBundles
{
    private const string ASSET_BUNDLES_PATH = "Assets/TestProject/AssetBundles";

    [MenuItem("Assets/Build AssetBundles")]
    private static void BuildAssetBundles()
    {
        BuildPipeline.BuildAssetBundles(ASSET_BUNDLES_PATH, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}
