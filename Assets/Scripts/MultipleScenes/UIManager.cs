using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace MultipleScenes
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] Image _fadeImage = null;
        [SerializeField] GameObject _dummyCamera = null;

        // UniTask fade 処理
        async public UniTask FadeInAlphaUniTask(float duration)
        {
            await _fadeImage.DOFade(endValue: 1, duration).SetEase(Ease.Linear);
        }
        async public UniTask FadeOutAlphaUniTask(float duration)
        {
            await _fadeImage.DOFade(endValue: 0, duration).SetEase(Ease.Linear);
        }

        // DummyCamera の切り替え
        public void SetDummyCamera(bool active)
        {
            _dummyCamera.SetActive(active);
        }
    }
}
