using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.TestProject.Scripts.Data;
using Assets.TestProject.Scripts.LoaderScene;
using System.Threading.Tasks;
using Assets.TestProject.Scripts.Infractructure.Interfaces;
using Assets.TestProject.Scripts.Infractructure.Loaders.Interfaces;
using Assets.TestProject.Scripts.UI;

namespace Assets.TestProject.Scripts.Infractructure
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private RemoteDatasURLCollector _remoteDatasURLCollector;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private float _fakeLoadTime = 2f;

        private LoadPreviewer _loadPreviewer;
        private IGameInfoManager _gameInfoManager;
        private IRemoteInfoLoader _remoteInfoLoader;
        private bool _gameIsLoaded;

        private void Awake()
        {
            _uiManager.UpdateRequested += UpdateUiManager;
        }

        private void OnDestroy()
        {
            _uiManager.UpdateRequested -= UpdateUiManager;
        }

        public void OnApplicationQuit()
        {
            if (!_gameIsLoaded)
                return;

            _uiManager.SaveInfoTo(_gameInfoManager.LoadedInfo);
            _gameInfoManager.SaveGameInfo();
        }

        public async UniTask<bool> LoadGameInfoAndSetupAsync()
        {
            _gameIsLoaded = false;
            _remoteInfoLoader = AllServices.GetService<IRemoteInfoLoader>();
            _gameInfoManager = AllServices.GetService<IGameInfoManager>();

            await SetupLoadPreviewver(5);
            _loadPreviewer.StartLoadAnim(true);

            try
            {
                GameInfo gameInfo = await LoadGameInfo();
                await LoadGameSettings(_remoteInfoLoader, gameInfo);
                HelloMessage helloMessage = await LoadHelloInfo(_remoteInfoLoader);
                await LoadAssetBundle();
                await FakeLoad();

                await _uiManager.SetupAsync(_loadPreviewer, helloMessage, gameInfo, _remoteDatasURLCollector.SimplesSceneBundleID, destroyCancellationToken);
                _loadPreviewer.StopLoadAnim();
                _gameIsLoaded = true;

                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }

        public void StartGame() => _uiManager.StartUI();

        private async UniTask<AssetBundle> LoadAssetBundle()
        {
            var assetBundle = await AllServices.GetService<IAssetBundleLoader>()
                .LoadAndCacheAsync(_remoteDatasURLCollector.SimplesSceneBundleID, destroyCancellationToken);
            _loadPreviewer.IncProgress();

            return assetBundle;
        }

        private async Task<GameInfo> LoadGameInfo()
        {
            var gameInfo = await _gameInfoManager.LoadGameInfoAsync(destroyCancellationToken);
            _loadPreviewer.IncProgress();
            return gameInfo;
        }

        private async Task<GameSettings> LoadGameSettings(IRemoteInfoLoader remoteInfoLoader, GameInfo gameInfo)
        {
            var gameSettings = await remoteInfoLoader.LoadAsync<GameSettings>(_remoteDatasURLCollector.GameSettingsID, destroyCancellationToken);

            if (gameSettings == null)
                gameSettings = gameInfo.GameSettings;
            else
                gameInfo.GameSettings = gameSettings;

            _loadPreviewer.IncProgress();
            return gameSettings;
        }

        private async Task<HelloMessage> LoadHelloInfo(IRemoteInfoLoader remoteInfoLoader)
        {
            var helloMessage = await remoteInfoLoader.LoadAsync<HelloMessage>(_remoteDatasURLCollector.HelloMessageID, destroyCancellationToken);
            _loadPreviewer.IncProgress();

            if (helloMessage == null)
                helloMessage = new HelloMessage("Hello base!");

            return helloMessage;
        }

        private async Task FakeLoad()
        {
            await UniTask.WaitForSeconds(_fakeLoadTime, cancellationToken: destroyCancellationToken);
            _loadPreviewer.IncProgress();
        }

        private async Task SetupLoadPreviewver(int maxDataCount)
        {
            await LoadPreviewer.LoadLoaderSceneAsync();
            _loadPreviewer = AllServices.GetService<LoadPreviewer>();
            _loadPreviewer.SetMaxLoadCount(maxDataCount);
        }

        private void UpdateUiManager() => UpdateUiManagerAsync().Forget();
        private async UniTaskVoid UpdateUiManagerAsync()
        {
            _loadPreviewer.SetMaxLoadCount(1);
            _loadPreviewer.StartLoadAnim();

            var newSettings = await LoadGameSettings(_remoteInfoLoader, _gameInfoManager.LoadedInfo);

            await _uiManager.UpdateInfo(_loadPreviewer, _remoteDatasURLCollector.SimplesSceneBundleID, newSettings);
            _loadPreviewer.StopLoadAnim();
        }
    }
}