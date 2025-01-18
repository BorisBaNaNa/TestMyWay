using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using JetBrains.Annotations;

namespace Assets.TestProject.Scripts.Infractructure
{
    public class GoogleDiskLoader : IRemoteInfoLoader
    {
        private const string LOAD_URL = "https://drive.google.com/uc?export=download&id=";

        [CanBeNull]
        public async UniTask<T> LoadAsync<T>(string id) where T : class
        {
            UnityWebRequest request = UnityWebRequest.Get(LOAD_URL + id);
            await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
                return null;
            }

            return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
        }

        public void Dispose() { }
    }
}