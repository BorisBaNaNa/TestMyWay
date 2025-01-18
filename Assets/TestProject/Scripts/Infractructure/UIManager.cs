using Assets.TestProject.Scripts.Data;
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

        public void Setup(HelloMessage helloMessage, GameSettings gameSettings, GameInfo _gameInfo)
        {
            if (helloMessage == null || gameSettings == null)
            {
                Debug.LogError("HelloMessage or GameSettings is null");
                return;
            }

            _helloMessageTMP.text = helloMessage.Message;
            int startCount = _gameInfo.CounterInfo == null ? gameSettings.StartingNumber : _gameInfo.CounterInfo.LastCount;
            _counter.Setup(startCount);
        }

        public void SaveInfoTo(GameInfo gameInfo)
        {
            gameInfo.CounterInfo = _counter.Info;
        }

        public void UpdateInfo(GameSettings gameSettings)
        {
            _counter.Setup(gameSettings.StartingNumber);
        }

        private void SendUpdateRequest() => UpdateRequested?.Invoke();
    }
}