using Assets.TestProject.Scripts.Data;
using Assets.TestProject.Scripts.Infractructure;
using Assets.TestProject.Scripts.Infractructure.Loaders.Interfaces;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.TestProject.Scripts.UI
{
    public class Counter : MonoBehaviour
    {
        public CounterInfo Info => new(Count);

        [SerializeField] private TextMeshProUGUI _counterTMP;
        [SerializeField] private Button _incrementBtn;
        [SerializeField] private string _buttonSpriteName;

        public int Count
        {
            get => _count;
            private set
            {
                _count = value;
                _counterTMP.text = $"Счёт: {value}";
            }
        }

        private int _count = 0;

        private void Awake()
        {
            _incrementBtn.onClick.AddListener(Inctement);
        }

        private void OnDestroy()
        {
            _incrementBtn.onClick.RemoveListener(Inctement);
        }

        public async UniTask SetupAsync(string simplesSceneBundleID, int startCount, CancellationToken token = default)
        {
            Count = startCount;

            Sprite buttonSprite = await AllServices.GetService<IAssetBundleLoader>()
                .LoadAssetAsync<Sprite>(simplesSceneBundleID, _buttonSpriteName, token);
            //_incrementBtn.image.overrideSprite = buttonSprite;
            _incrementBtn.image.sprite = buttonSprite;
        }

        private void Inctement() => ++Count;
    }
}