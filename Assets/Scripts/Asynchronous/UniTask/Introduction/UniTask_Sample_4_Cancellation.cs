using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;



/* キャンセル対応について
UniTask は Coroutine と違い、 Coroutine のように実行元の MonoBehaviour が消滅しても自動で止まらない。
UniTask は、手動で止める必要がある。 面倒と捉えることもあるが 大きなメリットでもある。
Coroutine は、停止したことに気づく手段がないのに対して、
UniTask は、キャンセルを例外として try- catch で補足ができる.
なので、キャンセルされたタイミングに応じて自由に後処理できる。

キャンセル対応しなければ、永久に UniTask が残る可能性があるので注意する
*/

/* CancellationToken について
UniTask メソッドに キャンセルされたことを伝えるために使用
*/

public class UniTask_Sample_4_Cancellation : MonoBehaviour
{
    async UniTask ExceptionExample(CancellationToken cancellationToken = default)
    {
        // 実行する 処理
        try
        {
            await UniTask.Delay(2000, cancellationToken: cancellationToken);
        }
        // Try にある 処理が実行されている最中に Cancel された時 行う 例外処理・後処理
        // catch が System.OperationCanceledException という例外を捕捉する
        catch (System.OperationCanceledException)
        {
            Debug.Log("例外処理実行");

            // 注意 ! System.OperationCanceledException を捕捉したので
            // もう一度 throw しておかないと呼び出し元でキャンセルを捕捉できなくなる
            throw;
        }
        // キャンセル されてもされなくても実行される処理
        finally
        {
            Debug.Log("finally 実行");
        }
    }

    #region  キャンセルテスト : MonoBehaviour が消滅したらキャンセルする例
    // CancellationTokenSource のインスタンス変数 を UniTask を実行する時に渡す
    // Forget の中のラムダ式は、キャンセルされた時の例外を捕捉しログをはきだすもの. Forget の中に、例外の補足を書き出す事ができる。
    private CancellationTokenSource cts = new CancellationTokenSource();

    void Start()
    {
        // 1. 実行最中に、 このクラスをアタッチしている Object を消すと、  OperationCanceledException: The operation was canceled. ログを確認できる
        // Example2(cts.Token).Forget((e) => Debug.LogException(e));

        // 2. 実は、Monobehavior の消滅によるキャンセルは もっと簡単に記述できる
        // これを使うと「AsyncAsyncDestoryTrigger」というコンポーネントが自動で追加される
        // 他の理由で キャンセル する場合は、CancellationTokenSource を使用する
        var ct = this.GetCancellationTokenOnDestroy();
        Example2(ct).Forget();
    }

    void OnDestroy()
    {
        // 消滅した時、Cancel を実行する事で、UniTask 側に キャンセルした事を伝える事が出来る
        cts.Cancel();
    }

    async UniTask Example2(CancellationToken ct = default)
    {
        // cts.Cancel() が実行された時、キャンセルされたことを伝えるためには
        // UniTask で用意されているメソッドには、 CancellToken を渡せば ok
        await UniTask.Delay(5000, cancellationToken: ct);

        // 上記の UniTask.Delay() が実行されている最中にキャンセルされた場合は、下記のの処理は実行されない

        // CancellationToken を渡せないメソッドの場合は、手動で例外をスローする
        GameObject prefab = await Resources.LoadAsync("Item1") as GameObject;
        // ThrowIfCancellationRequested() が呼ばれたタイミングで UniTask がキャンセルされている場合、例外を飛ばす
        ct.ThrowIfCancellationRequested();

        // Resources.LoadAsync()の実行中に キャンセルされた場合は、以降の処理は実行されない
        Instantiate(prefab);
    }

    #endregion
}
