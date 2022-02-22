using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/* メッセージを発行する方法

    UniRx において、イベントメッセージとして「OnNext メッセージ」を利用することが一般的な方法

    UniRxでは, Subject<T> を用いる以外の方法でも、メッセージを発行する方法がある。「AsyncSubject<T>」など。用途に合わせて使い分ける。

    UniRx.Subject を利用した例を下記に記述
*/
namespace UniRxSample
{
    public class SubjectSample : MonoBehaviour
    {
        void Start()
        {
            // subject 作成
            var subject = new Subject<string>();

            // Observer 登録
            var disposable = subject.Subscribe(x => Debug.Log(x));

            // OnNextメッセージを手動で発行
            subject.OnNext("Hello");
            subject.OnNext("World!");

            // 購読中止
            disposable.Dispose();
            // 購読が中止されているので、発行さるイベントを受信して処理を実行することはできない
            subject.OnNext( "Hello 2");


            subject.OnCompleted();
            subject.Dispose();

            // 上記で、subject を破棄したので メッセージを発行する処理が実行できない
            // ObjectDisposedException: Cannot access a disposed object.
            // subject.OnNext("Hello 2");
        }

    }
}
