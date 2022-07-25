using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Cysharp.Threading.Tasks.Triggers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ToRiQiita5
{
    public class CoroutineSample : MonoBehaviour
    {
        private void Start()
        {
            // ExecuteSample1();
            // ExecuteSample2();
            // ExecuteSample3();
            // StartCoroutine(WaitCoroutine());
            // ExecuteSample7();
            ExecuteSample8();
        }

        // 1~4 : コルーチンからIObservableに変換する
        // コルーチンをストリームに変換すれば、コルーチンの結果を使ってそのまま自然な流れでUniRxのオペレーターチェーンに接続してあげることが可能
        
        // 1. FromCoroutine : coroutine の終了タイミングをストリームとして待ち受ける
        //使うもの: Observable.FromCoroutine
        // 返り値:IObservable<Unit>
        // 第一引数:Func<IEnumerator> coroutine コルーチン本体
        // 第二引数:bool publishEveryYield = false yieldしたタイミングでOnNextを発行するか？ (falseでOnCompletedの直前に1度だけ発行,default = false)
        // REMARK : Observable.FromCoroutineはSubscribeされるごとに新たにコルーチンを生成して起動してしまうという点に注意。
        // コルーチンは1つだけ起動しストリームだけを共有して利用したいといった場合にはストリームのHot変換が必要になります。
        // Observable.FromCoroutineで起動したコルーチンはSubscribeをDisposeすると自動的に停止する
        // コルーチン上で自身のストリームがDisposeされたことを検知したい場合は、コルーチンの引数にCancellationTokenを渡すことでDisposeを検知することが可能
        // TODO : キャンセレーションToken の使用方法についてもっと学ぶ必要がある
        private void ExecuteSample1()
        {
            var disposable = Observable.FromCoroutine(token => HogeCoroutine(token), publishEveryYield: false)
                // 上記の Coroutine 終了後、下記の処理は実行される
                .Subscribe(
                    _ => Debug.Log("OnNext"),
                    () => Debug.Log("Completed"))
                .AddTo(this);

            // Dispose の検証用
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.D))
                .Subscribe(_ => disposable.Dispose())
                .AddTo(this);

            IEnumerator HogeCoroutine(CancellationToken token)
            {
                Debug.Log("coroutine started");
                //　なんか処理して待ち受けたり
                yield return new WaitForSeconds(3);
                Debug.Log("coroutine finished");
            }
        }

        // 2. FromCoroutineValue : コルーチンのyield returnの結果を取り出す
        // 返り値:IObservable<T>
        // 第一引数:Func<IEnumerator> coroutine コルーチン本体
        // 第二引数:bool nullAsNextUpdate = true nullの時にOnNextを発行しない,default = true
        // REMARK : yield return は呼び出されるたびに1フレーム停止するという性質がある
        // これを利用して1フレームずつ値を発行することが可能

        [SerializeField] private List<Vector2> moveList;

        private void ExecuteSample2()
        {
            Observable.FromCoroutineValue<Vector2>(MovePositionCoroutine).Subscribe(x => Debug.Log(x));

            IEnumerator MovePositionCoroutine()
            {
                foreach (var v in moveList)
                {
                    yield return v;
                }
                // もしくは
                // return moveList.GetEnumerator();
            }
        }

        // 3. FromCoroutine : コルーチン内部でOnNextを直接発行する
        // 返り値:IObservable<T>
        // 第一引数:Func<IObserver<T>, IEnumerator> coroutine IObserver<T>
        // コルーチン上の任意のタイミングでOnNextを発行することが可能になります。
        // この機能を利用することで、内部実装は手続き的な非同期処理で書きつつ、
        // 外部からストリームとして扱うといったコルーチンとUniRxの双方のいいとこ取り
        // REMARK : OnCompletedは自動で発行されないので、コルーチン終了のタイミングで自分でOnCompletedを発行してあげる必要がある
        // 「状態に依存した処理」や「途中で処理が大きく分岐する処理」といったものはUniRxのオペレータチェーンのみで記述するのは難しく、
        // 場合によっては記述不可能な場合もある。このようにコルーチンで内部実装を行いストリームに変換してしまうという方法を取ることを推奨。
        public bool IsPaused = false;

        private void ExecuteSample3()
        {
            Observable.FromCoroutine<long>(observer => CountCoroutine(observer))
                .Subscribe(x => Debug.Log(x))
                .AddTo(this);

            IEnumerator CountCoroutine(IObserver<long> observer)
            {
                long current = 0;
                float deltaTime = 0;
                // Disposeすると Coroutine は止まる。または、CancellationToke を利用
                while (true)
                {
                    if (!IsPaused)
                    {
                        deltaTime += Time.deltaTime;
                        if (deltaTime >= 1.0f)
                        {
                            // 差分が1秒を超えたら整数部を取り出して集計して通知
                            var integerPart = (int) Mathf.Floor(deltaTime);
                            current += integerPart;
                            deltaTime -= integerPart;
                            // 経過時間を通知
                            observer.OnNext(current);
                        }
                    }

                    yield return null;
                }
            }
        }

        // 4. FromMicroCoroutine : より低コストで軽量なコルーチンを実行
        // コルーチン内で yield return null しか利用できないとう制約がある代わりに、Unity標準のコルーチンと比べ非常に高速に起動し動作する仕組み

        private void ExecuteSample4()
        {
            var timer = Observable.FromMicroCoroutine<float>(observer => CountDownCoroutine(observer))
                .Subscribe(x => Debug.Log(x))
                .AddTo(this);


            IEnumerator CountDownCoroutine(IObserver<float> observer)
            {
                float currentTime = 0;
                while (currentTime <= 6)
                {
                    currentTime += Time.deltaTime;
                    observer.OnNext(currentTime);
                    yield return null;
                }

                observer.OnCompleted();
            }
        }
        
        // 1 ~ 4 : コルーチンからIObservableに変換するまとめ
        // REMARK : Observable.FromCoroutine等はSubscribeされた時点で新しくコルーチンを生成して起動してしまうため、
        // 1個のコルーチンを共有して複数回Subscribeする際はHot変換が必要
        
        
        // 5 ~ 8 : IObservableからコルーチンに変換する
        // コルーチン上でストリームの実行結果を待ち受けて、そのまま処理を続けるといった記述方法が可能
        
        // 5. ToYieldInstruction() (IObservable<T>に対する拡張メソッド) : ストリームをコルーチンに変換する
        // 返り値:ObservableYieldInstruction<T>
        // 引数:CancellationToken cancel 処理を中断した場合は引数に渡す(省略可)
        // 引数:bool throwOnError = false OnErrorが発生した時に例外内容をthrowするか?
        
        // REMARK : ToYieldInstructionはOnCompletedメッセージを受けてyield returnを終了します。
        // そのためOnCompletedを発行しない無限ストリームをToYieldInstructionしてしまうと永遠に終了しない状態になってしまうため注意が必要です。
        
        IEnumerator WaitCoroutine()
        {
            // Subscribeの代わりにToYieldInstruction()を利用することで
            // コルーチンとしてストリームを扱えるようになる
            Debug.Log("Wait for second");
            // coroutine 上で　ストリームの結果を待ち受ける
            yield return Observable.Timer(TimeSpan.FromSeconds(1)).ToYieldInstruction();
            // ToYieldInstruction()はOnCompletedが発行されてコルーチンを終了する
            // そのためOnCompletedが必ず発行されるストリームでのみ利用できる
            // 無限に続くストリームの場合はFirstやFirstOrDefaultを使うとよいかも
            Debug.Log("Press Any Key");
            yield return this.UpdateAsObservable()
                // FirstOrDefaultは条件を満たすとOnNextとOnCompletedを両方発行する
                .FirstOrDefault(_ => Input.anyKeyDown)
                .ToYieldInstruction();
            Debug.Log("Pressed button");
        }
        
        // 6. ストリーム中で発行されたOnNextメッセージを利用する場合(Unity 5.3以降))
        // ToYieldInstructionが返すObservableYieldInstruction<T>を変数に保存しておくことで結果の取得が可能になります。

        IEnumerator DetectCoroutine()
        {
            Debug.Log("Coroutine Start");
            // こルーチンが開始されてたから 3秒以内に最初に自分に触れたオブジェクトを取得する
            var o = 
                this.OnCollisionEnterAsObservable()
                    .FirstOrDefault()
                    .Select(x => x.gameObject)
                    // Timeoutは指定時間以内にストリームが終了しなかった場合に、OnErrorを発行するオペレータ
                    .Timeout(TimeSpan.FromSeconds(3))
                    .ToYieldInstruction(throwOnError: false);

            // 結果を待ち受ける
            yield return o;

            if (o.HasError || !o.HasResult)
            {
                // 3秒以内に何もヒットしなかった
                Debug.Log("hit object is nothing");
            }
            else
            {
                var hitObject = o.Result;
                Debug.Log(hitObject.name);
            }
        }
        
        // IObservableからコルーチンに変換するまとめ
        // ToYieldInstructionまたはStartAsCoroutineを利用することでストリームをコルーチンに変換することができる
        // 応用すれば「コルーチンの途中で特定のイベントの発行を待ち受ける」といったことが可能になる
        
        // 応用例
        // 7. コルーチンを直列に実行して待ち受ける
        private void ExecuteSample7()
        {
            Observable.FromCoroutine(CoroutineA)
                // //SelectManyで合成可能
                .SelectMany(CoroutineB)
                .Subscribe(_ => Debug.Log("All Coroutine Finished"));
        }
        IEnumerator CoroutineA()
        {
            Debug.Log("Start A");
            yield return new WaitForSeconds(3);
            Debug.Log("Finish A");
        }
        IEnumerator CoroutineB()
        {
            Debug.Log("Start B");
            yield return new WaitForSeconds(1);
            Debug.Log("Finish B");
        }
        
        // 8. 複数コルーチンを同時に起動して結果を待ち受ける

        private void ExecuteSample8()
        {
            // コルーチンAとコルーチンBを同時に起動し、全部終わるまで待ってから処理する
            Observable.WhenAll(
                    Observable.FromCoroutine<string>(o => CoroutineA(o)),
                    Observable.FromCoroutine<string>(o => CoroutineB(o)))
                .Subscribe(xs =>
                {
                    foreach (var x in xs)
                    {
                        Debug.Log($"result {x}");
                    }
                });
        }
        IEnumerator CoroutineA(IObserver<string> observer)
        {
            Debug.Log("Start A");
            yield return new WaitForSeconds(3);
            Debug.Log("Finished A");
            observer.OnNext("A Done !!");
            observer.OnCompleted();
        }
        IEnumerator CoroutineB(IObserver<string> observer)
        {
            Debug.Log("Start B");
            yield return new WaitForSeconds(1);
            Debug.Log("Finished B");
            observer.OnNext("B Done !!");
            observer.OnCompleted();
        }
        
        // 9. 重い処理を別スレッドに逃がしつつ、結果をコルーチン上で扱う 省略
    }
}

