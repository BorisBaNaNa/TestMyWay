using Assets.TestProject.Scripts.Data;
using Assets.TestProject.Scripts.Infractructure.Interfaces;
using Assets.TestProject.Scripts.LoaderScene;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.TestProject.Scripts.UI
{
    public class UIManager : MonoBehaviour, ISaveGameInfo
    {
        public event Action UpdateRequested;

        [SerializeField] private TextMeshProUGUI _helloMessageTMP;
        [SerializeField] private Counter _counter;
        [SerializeField] private Button _updateContentBtn;

        private void Awake()
        {
            _updateContentBtn.onClick.AddListener(SendUpdateRequest);
        }

        private void OnDestroy()
        {
            _updateContentBtn.onClick.RemoveListener(SendUpdateRequest);
        }

        public async UniTask SetupAsync(LoadPreviewer _loadPreviewer, HelloMessage helloMessage, GameInfo _gameInfo, string simplesSceneBundleID, CancellationToken token = default)
        {
            _loadPreviewer.SetMaxLoadCount(1);
            _helloMessageTMP.text = helloMessage.Message;

            try
            {
                int startCount = _gameInfo.CounterInfo == null ? _gameInfo.GameSettings.StartingNumber : _gameInfo.CounterInfo.LastCount;
                await _counter.SetupAsync(simplesSceneBundleID, startCount, token);
                _loadPreviewer.IncProgress();
            }
            catch (OperationCanceledException) { throw; }
        }

        public void StartUI()
        {
            gameObject.SetActive(true);
        }

        public void SaveInfoTo(GameInfo gameInfo)
        {
            gameInfo.CounterInfo = _counter.Info;
        }

        public async UniTask UpdateInfo(LoadPreviewer _loadPreviewer, string simplesSceneBundleID, GameSettings gameSettings, CancellationToken token = default)
        {
            try
            {
                _loadPreviewer.SetMaxLoadCount(1);
                await _counter.SetupAsync(simplesSceneBundleID, gameSettings.StartingNumber, token);
                _loadPreviewer.IncProgress();
            }
            catch (OperationCanceledException) { throw; }
        }

        private void SendUpdateRequest() => UpdateRequested?.Invoke();
    }
}