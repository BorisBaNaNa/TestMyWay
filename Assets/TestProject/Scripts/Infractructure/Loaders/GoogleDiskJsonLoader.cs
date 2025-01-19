using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using JetBrains.Annotations;
using Assets.TestProject.Scripts.Data;
using System.Threading;
using Assets.TestProject.Scripts.Infractructure.Loaders.Interfaces;

namespace Assets.TestProject.Scripts.Infractructure.Loaders
{
    public class GoogleDiskJsonLoader : IRemoteInfoLoader
    {
        [CanBeNull]
        public async UniTask<T> LoadAsync<T>(string id, CancellationToken token = default) where T : class
        {
            UnityWebRequest request = UnityWebRequest.Get(RemoteDatasURLCollector.GOOGLE_DISK_LOAD_URL + id);
            await request.SendWebRequest().ToUniTask(cancellationToken: token);

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning($"Error: {request.error}");
                return null;
            }

            return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
        }

        public void Dispose() { }
    }
}