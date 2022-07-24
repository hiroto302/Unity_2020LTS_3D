using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ToriQiita2
{
    // シーンの初期化完了を Unit で通知する
    public class GameInitializer : MonoBehaviour
    {
        // ゲームの初期化完了を通知する
        // ここではSubjectを利用したが、この場合ではAsyncSubjectの方が適切かもしれない
        private Subject<Unit> initializedSubject = new Subject<Unit>();
        public IObservable<Unit> OnInitializedAsync => initializedSubject;

        private void Start()
        {
            StartCoroutine(GameInitializeCoroutine());

            OnInitializedAsync.Subscribe(_ => Debug.Log("初期化完了"));
        }

        IEnumerator GameInitializeCoroutine()
        {
            // 初期化処理
            // オブジェクトのインスタンス化など重い処理をやることを想定

            yield return new WaitForSeconds(1);
            // 初期化終了を通知する
            initializedSubject.OnNext(Unit.Default);
            initializedSubject.OnCompleted();
        }
    }
}
