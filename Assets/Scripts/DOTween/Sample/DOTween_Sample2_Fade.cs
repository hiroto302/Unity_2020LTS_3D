using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DOTween_Sample2_Fade : MonoBehaviour
{
    [SerializeField] Image image = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            FadeOutAlpha(3.0f);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            FadeInAlpha(3.0f);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            FadeOutToIn();
        }
    }

    // alpha 1 => 0
    void FadeOutAlpha(float duration)
    {
        image.DOFade(endValue: 0, duration).SetEase(Ease.Linear);
    }
    // alpha 0 => 1
    void FadeInAlpha(float duration)
    {
        image.DOFade(endValue: 1, duration).SetEase(Ease.Linear);
    }

    Tween FadeOutAlpha2(float duration)
    {
        return image.DOFade(endValue: 0, duration).SetEase(Ease.Linear);
    }

    void FadeOutToIn()
    {
        FadeOutAlpha2(3.0f).OnComplete(() => FadeInAlpha(3.0f));
    }
}
