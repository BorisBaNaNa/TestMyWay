using Assets.TestProject.Scripts.Infractructure.Interfaces;
using Assets.TestProject.Scripts.Infractructure.Loaders;
using Assets.TestProject.Scripts.Infractructure.Loaders.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.TestProject.Scripts.Infractructure
{
    public class Bootstraper : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;

        private void Awake()
        {
            //Когда лень вспоминать или юзать Zenject
            AllServices.RegService<IRemoteInfoLoader>(new GoogleDiskJsonLoader());
            AllServices.RegService<IGameInfoManager>(new FileGameInfoManager());
            AllServices.RegService<IAssetBundleLoader>(new GoogleDockAssetBundleLoader());
        }

        private void Start()
        {
            _gameManager.LoadGameInfoAndSetupAsync().Forget();
        }

        private void OnDestroy()
        {
            AllServices.DisposeService<IRemoteInfoLoader>();
            AllServices.DisposeService<IGameInfoManager>();
            AllServices.DisposeService<IAssetBundleLoader>();
        }
    }
}
