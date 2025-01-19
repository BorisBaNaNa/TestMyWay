using Assets.TestProject.Scripts.Data;
using Assets.TestProject.Scripts.LoaderScene;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.TestProject.Scripts.Infractructure
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

        public async UniTask SetupAsync(LoadPreviewer _loadPreviewer, HelloMessage helloMessage, GameInfo _gameInfo, string simplesSceneBundleID)
        {
            _loadPreviewer.SetMaxLoadCount(1);
            _helloMessageTMP.text = helloMessage.Message;

            int startCount = _gameInfo.CounterInfo == null ? _gameInfo.GameSettings.StartingNumber : _gameInfo.CounterInfo.LastCount;
            await _counter.SetupAsync(simplesSceneBundleID, startCount);
            _loadPreviewer.IncProgress();
        }

        public void SaveInfoTo(GameInfo gameInfo)
        {
            gameInfo.CounterInfo = _counter.Info;
        }

        public async UniTask UpdateInfo(LoadPreviewer _loadPreviewer, string simplesSceneBundleID, GameSettings gameSettings)
        {
            _loadPreviewer.SetMaxLoadCount(1);
            await _counter.SetupAsync(simplesSceneBundleID, gameSettings.StartingNumber);
            _loadPreviewer.IncProgress();
        }

        private void SendUpdateRequest() => UpdateRequested?.Invoke();
    }
}