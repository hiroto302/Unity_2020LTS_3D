using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/* メッセージ購読について
UniRxにおいて、イベントメッセージを購読する場合、「IObservable<T>.Subscribe()にObserverオブジェクトを登録する」という操作が必要となる。
しかし、都度 Observerオブジェクトを定義し、インスタンス化するのはかったるい。(MySubjectUseSample.csでやったように)
そのため、UniRx では 「IObservable<T> に登録する Observerオブジェクトをデリゲートから生成できる拡張メソッド」が用意されている
この拡張メソッドを用いると、「ラムダ式で Subscribe() よび出す時に、イベントのハンドリング内容を記述する」ことが可能となる
*/
namespace UniRxSample
{
    public class SubscribeSample : MonoBehaviour
    {
        void Start()
        {
            // Subject を生成
            var subject = new Subject<string>();

            subject
                .Subscribe(
                    onNext: x => Debug.Log(x),
                    onError: ex => Debug.LogError(ex),
                    onCompleted: () => Debug.Log("OnCompleted")
                );

            subject.OnNext("Hello world!");
            subject.OnCompleted();
            subject.Dispose();
        }
    }
}
