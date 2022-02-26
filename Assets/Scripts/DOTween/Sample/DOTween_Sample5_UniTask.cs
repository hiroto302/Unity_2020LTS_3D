using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

/* UniTask で Tween の実行待機処理する方法
DoTween の await に対応させる方法動作させるためには次の設定が必要である
1. Scripting Define Symbolsに「UNITASK_DOTWEEN_SUPPORT」を定義
2. asmdef「UniTask.DOTween」への参照を追加 (UniTask.DOTween の アセンブリファイル を見つける。そして、Inspector で Apply を押す.)
これで OK!!
*/
public class DOTween_Sample5_UniTask : MonoBehaviour
{

    [SerializeField] Image _image = null;
    void Start()
    {
        FadeInToOut2();
    }

    // 1. fade In の実行が完了したら fade out させる処理
    async void FadeInToOut()
    {
        Debug.Log($"FadeIn 実行開始 : {Time.realtimeSinceStartup}");
        await FadeInAlpha(3.0f);
        Debug.Log($"FadeOut 実行開始 : {Time.realtimeSinceStartup}");
        await FadeOutAlpha(3.0f);
    }

    // 2. fade In している間に 行いたい処理を実行するのも 合わせてみる
    // WhenAll と 組み合わせることで シーンのロードなど 簡潔な記述で書けるようになる！
    async void FadeInToOut2()
    {
        await UniTask.WhenAll(FadeInAlpha(3.0f), Example(5.0f));
        await FadeOutAlpha(3.0f);
    }

    async UniTask FadeInAlpha(float duration)
    {
        await _image.DOFade(endValue: 1, duration).SetEase(Ease.Linear);
    }
    async UniTask FadeOutAlpha(float duration)
    {
        await _image.DOFade(endValue: 0, duration).SetEase(Ease.Linear);
    }

        async UniTask Example(float duration)
    {
        await transform.DOMoveX(5.0f, duration);
    }
}
