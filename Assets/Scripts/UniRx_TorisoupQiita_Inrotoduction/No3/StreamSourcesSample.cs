using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ToriQiita3
{
    
    /// <summary>
    /// ストリームソースを用意する方法
    /// 1. Subject シリーズ
    /// 2. ReactiveProperty シリーズ
    /// 3. ファクトリメソッドシリーズ
    /// 4. UniRx.Triggers シリーズ
    /// 5. Coroutine を変換して使う
    /// 6. uGUIイベントを変換して使う
    /// 7. その他 UniRx が用意しているもの
    /// </summary>
    public class StreamSourcesSample : MonoBehaviour
    {
        private void Start()
        {
            // ExecuteReactiveCollectionSample();
            // ExecuteObservableCreateSample();
            // ExecuteObservableTimerSample();
            ExecuteCoroutineSample();
        }

        // 2.1 ReactiveCollection : ReactivePropertyと同じようなものであり、状態の変化を通知する機能が内蔵されたList<T>
        // 要素の追加・削除・要素数の変化・上書き・移動・リストのクリア
        private void ExecuteReactiveCollectionSample()
        {
            var collection = new ReactiveCollection<string>();
            
            collection
                .ObserveAdd()
                .Subscribe(x =>
                {
                    Debug.Log(string.Format($"Add {x.Index} = {x.Value}"));
                });

            collection
                .ObserveRemove()
                .Subscribe(x =>
                {
                    Debug.Log(string.Format($"Remove {x.Index} = {x.Value}"));
                }, error => Debug.Log("削除失敗"));
            
            collection.Add("Apple");
            collection.Add("Banana");
            collection.Remove("Apple");
            // 削除することが出来ない時の処理などが気になる
            collection.Remove("Bana");
        }
        
        // 2.2 ReactiveDictionary<T1, T2> は、挙動がReactiveCollection とほぼ同じなので省略
        
        // 3. ファクトリメソッドシリーズ : ファクトリメソッドとは、UniRxが提供しているストリームソース構築メソッド群のこと
        // Subjectだけでは表現できないような複雑なストリームを簡単につくることができる場合があります。
        // UnityでUniRxを利用する場合はファクトリメソッドを利用する機会はそんなにないが、どこかで役に立つことがあると思うので基本を押さえておく
        
        // 3.1 Observable.Create
        private void ExecuteObservableCreateSample()
        {
            // REMARK : 下記のように ある特定のタイミング で 特定の処理を実行したい ! という処理を簡潔?? に記述できる。
            // 0~100 まで 10刻みで値を発行するストリーム
            Observable.Create<int>(observer =>
            {
                Debug.Log("Start");

                for (var i = 0; i <= 100; i += 10)
                {
                    // 10 ごとの値を発行 (特定のタイミング)
                    observer.OnNext(i);
                }

                Debug.Log("Finished");
                observer.OnCompleted();
                // ここがよくわからんけど、Completed が実行されると呼ばれていた。OnCompleted の次に呼ばれてた。
                return Disposable.Create(() =>
                {
                    // 終了時の処理
                    Debug.Log("Dispose");
                });
                // 上記で 発行されたタイミングで実行したい処理
            }).Subscribe(x => Debug.Log(x), () => Debug.Log("Complete"));
        }
        
        // 3.2 Observable.Start : 与えられたブロックを別スレッドで実行し結果を1つだけ発行するファクトリメソッド 省略
        
        // 3.3 Observable.Timer/TimerFrame : Timer は一定時間後にメッセージを発行するシンプルなファクトリメソッド
        // 実時間で指定する場合はTimerを、Unityのフレーム数で指定する場合はTimerFrameを利用。
        // Timer TimerFrameは引数によって挙動が変化。1個しか指定しない場合はOneShotな動作で終了し、2個指定した場合は定期的にメッセージを発行する挙動になります。
        // また、スケジューラを指定することで実行するスレッドを指定することも可能です。
        // また、似たファクトリメソッドしてObservable.Interval/IntervalFrameが存在。こちらはTimer/TimerFrame の2個引数を指定する場合の省略版みたいなものとなっています。
        // Interva/IntervalFrameではタイマを起動するまでの時間(第一引数)を指定することができなくなっています。

        private void ExecuteObservableTimerSample()
        {
            // 5秒後に、1回だけメッセージを発行して終了
            Observable.Timer(System.TimeSpan.FromSeconds(5))
                .Subscribe(_ => Debug.Log("5秒経過"));
            
            // ５秒後から、2秒ごとにメッセージを発行
            // REMARK : 自分で停止させない限り、ずっと動作し続ける。停止することを忘れないように。メモリリークやNullReferenceExceptionの原因となりうる
            Observable.Timer(System.TimeSpan.FromSeconds(5), System.TimeSpan.FromSeconds(2))
                .Subscribe(_ => Debug.Log("一定間隔で実行"))
                .AddTo(this);
        }
        
        // 4. UniRx.Triggersシリーズ : using UniRx.Triggers;を行うことで利用可能になるストリームソース。
        // UnityのコールバックイベントをUniRxのIObservableに変換して提供
        // Unityが提供するほとんどのコールバックイベントをストリームとして取得可能になっており、
        // またGameObjectがDestroyされた時にOnCompletedを自動で発行してくれる
        private void ExecuteTriggerSample()
        {
            var isForceEnabled = true;
            var rigidbody = GetComponent<Rigidbody>();
            
            // フラグが有効な間、上向きに力を加える
            this.FixedUpdateAsObservable()
                .Where(_ => isForceEnabled)
                .Subscribe(_ => rigidbody.AddForce(Vector3.up));
            
            // WarpZone に侵入したらフラグを有効にする
            this.OnTriggerEnterAsObservable()
                .Where(x => x.gameObject.CompareTag("WarpZone"))
                .Subscribe(_ => isForceEnabled = true);
            
            // WarpZone を出たらフラグを無効化する
            this.OnTriggerExitAsObservable()
                .Where(x => x.gameObject.CompareTag("WarpZone"))
                .Subscribe(_ => isForceEnabled = false);
        }
        
        // 5. コルーチンから変換 : コルーチンからIObservableに変換するにはObservable.FromCoroutineを利用する
        // オペレータチェーンでゴリ押しで複雑なストリームを構築するより、コルーチンを併用して手続き的に書いた場合の方がシンプルでわかりやすく書ける場合も存在する
        // ここでは、簡単なサンプルを記述
        
        // 一時停止フラグ
        public bool IsPaused { get; private set; } = false;

        void ExecuteCoroutineSample()
        {
            Observable.FromCoroutine<int>(observer => GameTimerCoroutine(observer, 60)).Subscribe(t => Debug.Log(t));

            // 初期値から0までカウントするCoroutine. ただし、一時停止フラグが有効な場合カウントしない
            IEnumerator GameTimerCoroutine(IObserver<int> observer, int initialCount)
            {
                var currentCount = initialCount;
                while (currentCount > 0)
                {
                    if (!IsPaused)
                    {
                        observer.OnNext(currentCount--);
                    }

                    yield return new WaitForSeconds(1);
                }
                observer.OnNext(0);
                observer.OnCompleted();
            }
        }
        
        // 6. uGUIから変換 疲れたからまたそのうち
        
        // 7. その他
        
        // 7.1 ObserveEveryValueChanged : 任意のオブジェクトのパラーメタを毎フレーム監視して変化があった時に通知するストリームを作成
        private void ExecuteObserveEveryValueChangedSample()
        {
            var characterController = GetComponent<CharacterController>();
            characterController
                .ObserveEveryValueChanged(c => c.isGrounded)
                .Where(x => x)
                .Subscribe(_ => Debug.Log("着地"))
                .AddTo(this);
            
            // 上記とほぼ同意の記述
            Observable.EveryUpdate()
                .Select(_ => characterController.isGrounded)
                .DistinctUntilChanged()
                .Where(x => x)
                .Subscribe(x => Debug.Log($"着地 {x}"))
                .AddTo(this);
            
            // // ObserveEveryValueChangedは
            // EveryUpdate + Select + DistinctUntilChanged
            // の省略記法と思ってよい
        }
        
        
        
    }
}
