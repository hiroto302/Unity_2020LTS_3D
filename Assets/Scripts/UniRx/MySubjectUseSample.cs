using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniRxSample
{
    // MySubject の使用例
    public class MySubjectUseSample : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // MySubject の作成
            var mySubject = new MySubject<string>();

            // Observer 登録
            var disposable = mySubject.Subscribe(new DisplayLogObserver());

            // イベント の発行
            mySubject.OnNext("Hello");
            mySubject.OnNext("World");

            // 購買中止
            disposable.Dispose();

            mySubject.OnCompleted();
            mySubject.Dispose();
        }
    }
}
