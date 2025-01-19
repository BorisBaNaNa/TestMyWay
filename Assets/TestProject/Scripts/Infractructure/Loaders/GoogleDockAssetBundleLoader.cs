using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Assets.TestProject.Scripts.Data;
using System.Collections.Generic;
using System.Threading;
using Assets.TestProject.Scripts.Infractructure.Loaders.Interfaces;

namespace Assets.TestProject.Scripts.Infractructure.Loaders
{
    public class GoogleDockAssetBundleLoader : IAssetBundleLoader
    {
        private readonly Dictionary<string, AssetBundle> _loadedBundles = new Dictionary<string, AssetBundle>(); // <bundleId, bundle>

        public async UniTask<AssetBundle> LoadAndCacheAsync(string bundleId, CancellationToken token = default)
        {
            if (_loadedBundles.TryGetValue(bundleId, out var cachedBundle))
                return cachedBundle;

            await UniTask.WaitWhile(() => !Caching.ready, cancellationToken: token);

            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(RemoteDatasURLCollector.GOOGLE_DISK_LOAD_URL + bundleId);
            await request.SendWebRequest()
                .ToUniTask(cancellationToken: token);

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning($"Bundle load error: {request.error}");
                return null;
            }

            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            _loadedBundles.Add(bundleId, assetBundle);
            return assetBundle;
        }

        public async UniTask<T> LoadAssetAsync<T>(string bundleId, string assetName, CancellationToken token = default) where T : Object
        {
            var bundle = await LoadAndCacheAsync(bundleId, token);

            if (bundle == null)
                return null;

            return await bundle.LoadAssetAsync<T>(assetName).ToUniTask(cancellationToken: token) as T;
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