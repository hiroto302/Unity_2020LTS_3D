using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/* IReadOnlyReactiveProperty インターフェースについて
ReactiveProperty<T> は、IReadOnlyReactiveProperty<T>というインターフェースを実装している。
これは、「値の読み取り」および「IObservable<T>としての購読」を提供するインターフェースである
ReactiveProperty<T> をクラス外部に公開したいが、値の書き込みはクラス内のみに限定したい場合、このインターフェースにキャストすることが有効的である
*/

namespace UniRxSample
{
    // 毎秒カウントアップするタイマーを提供するクラス
    public class CountUpTimerProvider : MonoBehaviour
    {
        // ReactiveProperty 本体
        readonly ReactiveProperty<int> _timerCount = new ReactiveProperty<int>(0);

        // IReadOnlyReactiveProperty<int> にキャストして外部のクラスに公開
        public IReadOnlyReactiveProperty<int> TimerCount => _timerCount;

        void Start()
        {
            // TimerCount.Subscribe(time => Debug.Log(time));
            // コルーチンで毎秒カウントアップ
            StartCoroutine(TimerCoroutine());
        }

        private IEnumerator TimerCoroutine()
        {
            while(true)
            {
                _timerCount.Value++;
                yield return new WaitForSeconds(1);
            }
        }

        private void OnDestroy()
        {
            // 破棄
            _timerCount.Dispose();
        }
    }
}

