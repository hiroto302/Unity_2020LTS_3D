using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers; // この using を記述すること

/* MonoBehaviour のイベントからの変換
MonoBehaviour に定義されている各種イベントを、Observable に変換することができる。
これは、オブジェクトと結びついていることになるので、GameObjectが Destroy されたタイミングで、
自動的に OnCompleted メッセージを発行する

そのイベント内で処理が実行された時、メッセージを発行するということ? あとで要検討
*/

namespace UniRxSample
{
    public class MonoBehaviourEventToObservable : MonoBehaviour
    {
        void Start()
        {
            // 下記に一部例を記載
            // 各種Update系から変換
            this.UpdateAsObservable().Subscribe();
            this.FixedUpdateAsObservable().Subscribe();
            // OnCollision系から変換
            this.OnCollisionEnterAsObservable().Subscribe();
            // OnDestroy系から変換
            this.OnDestroyAsObservable().Subscribe();
        }

        // void Update()
        // {
        //     Debug.Log("実行されているよ");
        // }
    }
}

