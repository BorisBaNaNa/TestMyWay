using Assets.TestProject.Scripts.Infractructure;
using Assets.TestProject.Scripts.Infractructure.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.TestProject.Scripts.LoaderScene
{
    public class LoadPreviewer : MonoBehaviour, IService
    {
        public const string SCENE_NAME = "LoadScene";

        [SerializeField] private CanvasGroup _mainCanvasGroup;
        [SerializeField] private Image _progressBar;
        [SerializeField] private RectTransform _loadIcon;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _sectionAnimDuration = 0.5f;

        private Sequence _loadSequence;
        private int _maxLoadCount;
        private float _loadedCount;

        public static async UniTask LoadLoaderSceneAsync()
        {
            await SceneManager.LoadSceneAsync(SCENE_NAME, LoadSceneMode.Additive).ToUniTask();

            var instance = FindAnyObjectByType<LoadPreviewer>();
            AllServices.RegService<LoadPreviewer>(instance);
        }

        public void Dispose()
        {
            SceneManager.UnloadSceneAsync(SCENE_NAME);
        }

        public void SetMaxLoadCount(int count)
        {
            _maxLoadCount = count;
            _loadedCount = 0;
            _progressBar.fillAmount = 0f;
        }

        public void IncProgress()
        {
            _progressBar.DOKill();
            _progressBar.DOFillAmount(Mathf.Clamp01((float)++_loadedCount / _maxLoadCount), 0.2f);
        }

        public void StartLoadAnim(bool immediately = false)
        {
            _progressBar.fillAmount = 0f;
            _loadIcon.localScale = new Vector3(1f, 5f, 1f);
            gameObject.SetActive(true);
            _loadSequence = DOTween.Sequence()
                .Append(_loadIcon.DOScaleX(5f, _sectionAnimDuration))
                .Join(_loadIcon.DOScaleY(1f, _sectionAnimDuration))
                .Append(_loadIcon.DOScaleY(5f, _sectionAnimDuration))
                .Join(_loadIcon.DOScaleX(1f, _sectionAnimDuration))
                .SetLoops(-1);

            if (!immediately)
            {
                _mainCanvasGroup.DOFade(1f, _fadeDuration)
                    .From(0f)
                    .SetEase(Ease.Linear);
            }
        }

        public void StopLoadAnim()
        {
            _mainCanvasGroup.DOFade(0f, _fadeDuration)
                .From(1f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    _loadSequence.Kill();
                    SetMaxLoadCount(0);
                });
        }
    }
}
