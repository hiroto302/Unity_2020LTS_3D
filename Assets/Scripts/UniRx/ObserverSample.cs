using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* IObserver インターフェースについて
IObserver<T> は、「イベントメッセージを受信できる」という振る舞いが定義されたインターフェース。
「Observer オブジェクト」を表現する時に用いられる

IObserver<T> インターフェースを継承したクラスは、以下の３つのインターフェースメソッドを実装する必要がある
OnNext[T value]          : 新しくイベントメッセージを受信する
OnError[Exception error] : 発生した例外(エラー)を受信する
OnCompleted[]            : イベントメッセージの発行が全て完了したという通知を受信する

*/

/* IObserver<T> を実装する例 1
Sample1 では,IObserver<T> を経由して、受信したメッセージを表示する例を実装する
*/
namespace UniRxSample
{
    // sealed : 継承されることを禁止することができる
    public sealed class ObserverSample : MonoBehaviour
    {
        void Start()
        {
            // Observer を手動生成 (IOserver<string> を継承したクラスをインスタンス化)
            IObserver<string> observer = new DisplayLogObserver();
            // メッセージを発行する
            observer.OnNext("Hello");
            observer.OnNext("World");
            observer.OnCompleted();
        }
    }

    // 発行されたイベントメッセージを受信する Observer クラス
    public sealed class DisplayLogObserver : IObserver<string>
    {
        public void OnCompleted()
        {
            Debug.Log("ログの発行完了");
        }
        public void OnError(Exception error)
        {
            Debug.Log($"例外が発生 : {error}");
        }
        public void OnNext(string value)
        {
            Debug.Log($"メッセージを発行 : {value}");
        }
    }
}
