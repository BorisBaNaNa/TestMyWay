using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Assets.TestProject.Scripts.Data;
using Assets.TestProject.Scripts.Infractructure.Interfaces;
using System.Collections.Generic;

namespace Assets.TestProject.Scripts.Infractructure
{
    public class AssetBundleLoader : IService
    {
        private readonly Dictionary<string, AssetBundle> _loadedBundles = new Dictionary<string, AssetBundle>(); // <bundleId, bundle>

        public async UniTask<AssetBundle> LoadAndCacheAsync(string bundleId)
        {
            if (_loadedBundles.TryGetValue(bundleId, out var cachedBundle))
                return cachedBundle;

            await UniTask.WaitWhile(() => !Caching.ready);

            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(RemoteDatasURLCollector.GOOGLE_DISK_LOAD_URL + bundleId);
            await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning($"Bundle load error: {request.error}");
                return null;
            }

            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            _loadedBundles.Add(bundleId, assetBundle);
            return assetBundle;
        }

        public async UniTask<T> LoadAssetAsync<T>(string bundleId, string assetName) where T : Object
        {
            var bundle = await LoadAndCacheAsync(bundleId);

            if (bundle == null)
                return null;

            return await bundle.LoadAssetAsync<T>(assetName).ToUniTask() as T;
        }

        public void Dispose()
        {
            foreach (var bundle in _loadedBundles.Values)
                bundle.Unload(true);
            _loadedBundles.Clear();
            Caching.ClearCache();
        }
    }
}