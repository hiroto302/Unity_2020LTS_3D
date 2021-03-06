using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/* 購読終了 と キャンセル
Observable の購読(イベント待ち受け)を終了する方法 は3つある。それぞれの sample に記述していく
*/

/* 1. Subscribe().Dispose() の実行
IObservable<T>.Subscribe()を実行した時、返り値として IDisposable が返される。
この、IDisposable.Dispose() を実行することで、購読処理を中断することができる。
*/

/* 2. OnCompleted / OnError メッセージを発行する
Observable からOnCompleted メッセージ または OnError メッセージが発行されると、その Observable は「終了した」という扱いになる
これらのメッセージを受信したタイミングで、Subscribe() で登録した Observer の登録が解除される。
この仕組みを利用し、Observable を自分で定義する場合、使い終わったタイミングで OnCompleted を実装することもオススメである。

下記のサンプルのような、 Observable (EverUpdate()を利用したやつ)は、OnCompleted を発行することなく、無限に動作するものもあるので Dispose() 処理をよび出すこと。
*/

/* 3. ストリームソース側を Dispose()する
ストリームソース(Observerable の根源、Subject など)側に、Dispose()が定義されている場合、それをよび出すことで購読を終了できる
この場合は、ストリームソースに登録されている全ての Observer 登録が解除される
*/


namespace UniRx
{
    public class DisposeSample : MonoBehaviour
    {
        private IDisposable _disposable;

        void Start()
        {
            // 毎フレーム、前に移動させる
            // Subscribe()時に返される IDisposable を保持しておく
            _disposable = Observable.EveryUpdate()
                .Subscribe(_ =>
                    {
                        transform.position += Vector3.forward * Time.deltaTime;
                    }
                ).AddTo(this);
            // 上記のように, AddTo メソッドを利用することで、IDisposable.Dispose() の実行をGameObject または Component の寿命に紐付けることができる。
            // 下記の記述が必要なくなる
        }

        void Update()
        {
            // 検証 : 実際に購読を終了して、毎フレーム前に移動しなくなるか
            if (Input.GetKeyDown(KeyCode.D))
            {
                _disposable.Dispose();
            }
        }

        void OnDestroy()
        {
            // Destroy時に確実に購読を止める
            _disposable?.Dispose();
        }
    }
}
