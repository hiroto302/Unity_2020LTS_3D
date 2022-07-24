using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ToriQiita2
{
    /// <summary>
    /// IObservableの「IDisposable」について
    /// IDisposableはC#に用意されている既定のインターフェースであり、
    /// 「リソースの開放」を行えるようにするためのメソッド「Dispose()」をただ１つ持つインターフェースです。
    /// Subscribeの返り値がIDisposableということはつまり、
    /// Subscribeメソッドが返すIDisposabelのDisposeを実行すればストリームの購読を終了できるということになります。
    ///
    /// REMARK :
    /// このように、Disposeを呼び出すことで購読を任意のタイミングで停止することができる。
    /// ここで注意してほしいのは、Dispose()を実行して購読中止した場合、OnCompletedが発行されるわけではないという点。
    /// 購読中止時の処理をOnCompletedに書いていた場合、Disposeで停止させてしまうと実行されないことになってしまうので注意して下さい。
    ///
    /// ストリームの寿命管理は細心の注意を払い、使い終わったら必ずDisposeを呼ぶ、またはOnCompletedを発行する癖をつける!!
    /// (Subjectが破棄されればストリームも全て破棄される。逆に言えば、Subjectが残っている限りストリームは稼働し続けてしまうということになります。
    /// ストリームが参照しているオブジェクトをストリームより先に破棄して放置してしまうと、裏でストリームが動き続けたままのせいでパフォーマンスを低下させたり、
    /// メモリリークを引き起こしたり、NullReferenceExceptionを発生させてゲームを停止させてたりしてしまう可能性が出てきます。)
    /// </summary>
    public class DisposeSample : MonoBehaviour
    {
        private void Start()
        {
            var subject = new Subject<int>();

            var disposable = subject.Subscribe(n => Debug.Log($"{n}"), (() => Debug.Log($"complete")));
            var disposable2 = subject.Subscribe(n => Debug.Log($"{n}"), (() => Debug.Log($"complete")));
            
            
            // 1 は 2回表示される
            subject.OnNext(1);
            
            // Dispose : 特定のストリームのみ購読を終了
            // Dispose したら, OnCompleted が発行さレないことが確認できる
            disposable.Dispose();
            // 一つ 解放したので、１回表示される
            subject.OnNext(2);
            // OnCompleted : 全ての購読を終了
            subject.OnCompleted();
            disposable2.Dispose();
        }
    }
}
