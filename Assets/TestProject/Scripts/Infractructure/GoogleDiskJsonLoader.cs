using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using JetBrains.Annotations;
using Assets.TestProject.Scripts.Data;
using System.Collections.Generic;

namespace Assets.TestProject.Scripts.Infractructure
{
    public class GoogleDiskJsonLoader : IRemoteInfoLoader
    {
        [CanBeNull]
        public async UniTask<T> LoadAsync<T>(string id) where T : class
        {
            UnityWebRequest request = UnityWebRequest.Get(RemoteDatasURLCollector.GOOGLE_DISK_LOAD_URL + id);
            await request.SendWebRequest().ToUniTask();

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