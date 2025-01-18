using Assets.TestProject.Scripts.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.TestProject.Scripts.Infractructure
{
    public class Counter : MonoBehaviour
    {
        public CounterInfo Info => new(Count);

        [SerializeField] private TextMeshProUGUI _counterTMP;
        [SerializeField] private Button _incrementBtn;

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

        public void Setup(int startCount)
        {
            Count = startCount;
        }

        private void Inctement() => ++Count;
    }
}