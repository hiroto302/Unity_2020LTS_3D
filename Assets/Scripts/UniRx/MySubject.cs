using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/* Subject オブジェクト・ IObservable インターフェースについて
IObservable<T>は、「IObserver<T>を登録できる」とういう振る舞いが定義されたインターフェース
IObserver<T> が、メッセージを受信する場合に用いるインターフェースのに対して、こちらは送出側で用いるインターフェースと言える。

IObservable<T> インターフェースのメソッド
Subscribe[ IObserver<T> observer] : IObserver<T>を受け付けて自身に登録する。戻り値の IDisposable.Dispose() を実行することで登録を解除できる。

上記のメソッドの通り、IObservable<T> を実装したクラスは、IObserver<T>を登録できるようになる。つまり、「イベントメッセージを発行できる」と同義である。

Subjectは、Observerオブジェクトを登録・管理し、イベントメッセージを発行を実行することができるオブジェクトである。
「イベントリスナを登録して、イベントメッセージを発行する」という、イベント処理の根幹の役割を担っている。
UniRx においては、ISubject<T>インターフェースが定義されており、このインターフェースを実装している場合は、Subject であると扱われます。
UniRx における Subject の特徴として、IObserver<T> と IObservable<T> の２つが実装している点である。
Subjectは、IObserver<T> をメッセージ入力、IObservable<T>を送出に利用している。

下記には、Subject の動作原理を簡略化したものを実装する。
実際は、UniRx.Subject を利用すること
*/

namespace UniRxSample
{
    // Subject の実装例
    public sealed class MySubject<T> : ISubject<T>, IDisposable
    {
        // 排他ロック用のオブジェクト
        readonly object _gate = new object();
        // 登録された Observer 一覧
        List<IObserver<T>> _observers;
        // この Subject が破棄されたか
        bool _isDisposed;
        // この Subject が停止状態となっているか
        bool _isStopped;
        // 最後に発行された例外
        Exception _error;

#region  IObserver<T>
        public void OnNext(T value)
        {
            lock (_gate)
            {
                ThrowIfDisposed();
                if (_isStopped) return;
                if (_observers != null)
                {
                    foreach (var observer in _observers)
                    {
                        // イベントメッセージを各Observer に伝える
                        observer.OnNext(value);
                    }
                }
            }
        }

        public void OnError(Exception error)
        {
            lock (_gate)
            {
                ThrowIfDisposed();
                if (_isStopped) return;

                _error = error;

                if (_observers != null)
                {
                    foreach (var observer in _observers)
                    {
                        // 例外を各Observer に伝える
                        observer.OnError(error);
                    }

                    _observers.Clear();
                    _observers = null;
                }
                // 動作停止
                _isStopped = true;
            }
        }
        public void OnCompleted()
        {
            lock (_gate)
            {
                ThrowIfDisposed();
                if (_isStopped) return;
                if (_observers != null)
                {
                    foreach (var observer in _observers)
                    {
                        // 完了を各Observer に伝える
                        observer.OnCompleted();
                    }
                    _observers.Clear();
                    _observers = null;
                }
                // 動作停止
                _isStopped = true;
            }
        }
#endregion

#region  IObservable<T>
        // IObserver<T> をSubject に登録する処理
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException();

            lock (_gate)
            {
                ThrowIfDisposed();

                if (!_isStopped)
                {
                    // Observerのリストに登録する
                    if (_observers == null) _observers = new List<IObserver<T>>();
                    _observers.Add(observer);

                    // IDisposable.Disposed()で購読中断できるように
                    // inner class でラップする
                    return new Subscription(observer, this);
                }
                else
                {
                    // 異常終了/正常終了しているならそれを伝えて終了
                    if (_error != null)
                    {
                        observer.OnError(_error);
                    }
                    else
                    {
                        observer.OnCompleted();
                    }
                    return Disposable.Empty;
                }
            }
        }
#endregion

        void ThrowIfDisposed()
        {
            lock (_gate)
            {
                if (_isDisposed) throw new ObjectDisposedException(nameof(GetType));
            }
        }

        public void Dispose()
        {
            lock (_gate)
            {
                if (_isDisposed) return;
                _isDisposed = true;
                _observers = null;
            }
        }

        // 購買状態を管理する inner class
        // Dispose() を呼ぶと Subject から Observer を削除する
        private sealed class Subscription : IDisposable
        {
            private readonly IObserver<T> _observer;
            private MySubject<T> _parent;
            private readonly object _gate = new object();

            public Subscription(IObserver<T> observer, MySubject<T> parent)
            {
                _observer = observer;
                _parent = parent;
            }
            public void Dispose()
            {
                lock (_gate)
                {
                    if (_parent == null ) return;
                    lock (_parent._gate)
                    {
                        // Observer の登録解除
                        _parent._observers?.Remove(_observer);
                        _parent = null;
                    }
                }
            }
        }
    }
}
