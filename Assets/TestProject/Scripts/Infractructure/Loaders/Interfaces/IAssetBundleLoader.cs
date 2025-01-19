using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.TestProject.Scripts.Infractructure.Interfaces;
using System.Threading;

namespace Assets.TestProject.Scripts.Infractructure.Loaders.Interfaces
{
    public interface IAssetBundleLoader : IService
    {
        UniTask<AssetBundle> LoadAndCacheAsync(string bundleId, CancellationToken token = default);
        UniTask<T> LoadAssetAsync<T>(string bundleId, string assetName, CancellationToken token = default) where T : Object;
    }
}