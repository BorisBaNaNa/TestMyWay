using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.TestProject.Scripts.Infractructure
{
    public class Bootstraper : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;

        private void Awake()
        {
            //Когда лень вспоминать или юзать Zenject
            AllServices.RegService<IRemoteInfoLoader>(new GoogleDiskLoader());
            AllServices.RegService<IGameInfoManager>(new FileGameInfoManager());
        }

        private void Start()
        {
            _gameManager.LoadGameInfoAndSetupAsync().Forget();
        }

        private void OnDestroy()
        {
            AllServices.DisposeService<IRemoteInfoLoader>();
            AllServices.DisposeService<IGameInfoManager>();
        }
    }
}
