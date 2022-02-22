using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/* メッセージ購読について
UniRxにおいて、イベントメッセージを購読する場合、「IObservable<T>.Subscribe()にObserverオブジェクトを登録する」という操作が必要となる。
しかし、都度 Observerオブジェクトを定義し、インスタンス化するのはかったるい。(MySubjectUseSample.csでやったように)
そのため、UniRx では 「IObservable<T> に登録する Observerオブジェクトをデリゲートから生成できる拡張メソッド」が用意されている
この拡張メソッドを用いると、「ラムダ式で Subscribe() よび出す時に、イベントのハンドリング内容を記述する」ことが可能となる

Subscribe 購読 : 観察者?が発したメッセージを購読（受け取る）する。(サイトによって、Observer と Subject との関係性の説明が違う)
ブロードキャスト : 通信・ネットワークの分野ではネットワークに参加するすべての機器に同時に信号やデータを送信することを意味する
*/
namespace UniRxSample
{
    public class SubscribeSample : MonoBehaviour
    {
        void Start()
        {
            // Subject を生成
            var subject = new Subject<string>();

            // subject に Observer オブジェクトを登録する。イベントが発行され時、 subjectに 登録されている observer に イベントメッセージのブロードキャストが行われる。
            // そのブロードキャスト(メッセージ) を受信(購読している)した observer が実行する、イベントのハンドリング内容を下記に記述する
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
