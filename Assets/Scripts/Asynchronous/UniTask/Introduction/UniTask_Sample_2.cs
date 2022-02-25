using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;             // CancellationToken に必要

/* コルーチン と 同等の機能も用意
yield return null : UniTask.Yield()
new WaitUntil()   : UniTask.WaitUnitil()
new WaitWhile()   : UniTask.WaitWhile()
*/

// コルーチン自体も await で待てるので、 UniTask から コルーチンを呼ぶこともできる

/* UniTask Tracker
現在動いている UniTask をモニタリング
UniTask が消えずに残っている事があればここで分かる

*/

public class UniTask_Sample_2 : MonoBehaviour
{
    bool _isWait;

    void Start()
    {
        // 1. 呼び出し方
        // Coroutine
        StartCoroutine(CoroutineExample());

        // UniTask
        // 戻り値UniTaskの関数は、普通の関数と同じように書ける
        // Forget() をつけることで await キーワードなしで呼べる。Start() に async キーワードついていない時など使える。
        // .Forget()関数無しでも、動作はしますが以下のような警告が出る
        // 「この関数は非同期(async)だから、呼んだ関数が終わるまで待たないけど、大丈夫？」みたいな警告
        // つまり、実行する UniTask 処理の完了を待たずに、同期的に次の処理へ進むけど いいのか? ってこと.
        // Forget() をつけることで 大丈夫であることを伝える
        UniTaskExample().Forget();
    }

    // 1. コルーチン処理 : yield return null,
    IEnumerator CoroutineExample()
    {
        yield return null;
        Debug.Log("CoroutineExample 実行完了");
    }
    // 1. UniTask バージョン
    async UniTask UniTaskExample()
    {
        // PlayerLoopTiming.Update : 関数の更新タイミングをUnity標準のUpdate関数に合わせることを指定
        await UniTask.Yield(PlayerLoopTiming.Update);
    }

    // その他の比較
    async UniTask UniTaskWaitExample(CancellationToken cancellation_token)
    {
        /* yield return new WaitForSeconds(1);
        第一引数 : ミリ秒
        第二引数 : timeScale を無視するか
        第三引数 : 関数の更新タイミング、デフォルトでは Update 関数に合わせている
        第四引数 : CancellationTokenは「処理を途中で止める」のに必要な引数
        */
        await UniTask.Delay(1000);
        await UniTask.Delay(1000, false, PlayerLoopTiming.Update, cancellation_token);

        /* yield return new WaitWhile()
        yield return new WaitWhile( () => _isWait);
        */
        await UniTask.WaitWhile(() => _isWait);

        // コルーチンを呼び出し、実行完了を待つ記述
        await CoroutineExample();

        // 他のUniTask を呼び出し、実行完了まで待機
        await UniTaskExample();
    }



}
