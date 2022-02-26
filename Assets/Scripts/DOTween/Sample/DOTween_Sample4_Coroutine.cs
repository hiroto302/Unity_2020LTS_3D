using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// コルーチンやTaskで DOTweenを待機する方法
// async operation が返り値 に出来るメソッド
public class DOTween_Sample4_Coroutine : MonoBehaviour
{
    void Start()
    {
        // StartCoroutine(Example());
        StartCoroutine(Example2());
    }

    // 1. WaitForCompletion() : Tween 処理が完了 または 停止(強制終了) するまで 待機するyield命令を作成 (async operation が返り値)
    IEnumerator Example()
    {
        Debug.Log($"実行開始 : {Time.realtimeSinceStartup}");
        yield return transform.DOMoveX(5.0f, 2.0f).WaitForCompletion();
        Debug.Log($"実行完了 : {Time.realtimeSinceStartup}");
    }

    // 2. WaitForElapsedLoops() : 繰り返し実行される Tween が終了するか、指定された回数のループが繰り返されるまで待機するyield命令を作成
    IEnumerator Example2()
    {
        Debug.Log($"実行開始 : {Time.realtimeSinceStartup}");
        Tween myTween = transform.DOMoveX(5, 2).SetLoops(4);
        yield return myTween.WaitForElapsedLoops(2);
        Debug.Log($"2回 実行完了 : {Time.realtimeSinceStartup}");
    }

    // 3. WaitForKill
    // 4. WaitForPosition
    // 5. WaitForRewind
    // 6. WaitForStart      が用意されていた。必要に応じて使おう


}
