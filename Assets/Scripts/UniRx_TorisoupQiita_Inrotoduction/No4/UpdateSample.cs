using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ToriQiita4
{
    /// <summary>
    /// Update ストリーム　の利用サンプル
    /// Updateをストリームに変換するメリット
    /// UniRxのオペレータを利用してロジックを記述できるようになる
    /// ロジックの処理単位が明確になる　
    /// </summary>
    public class UpdateSample : MonoBehaviour
    {
        // ボタンを押している間一定間隔で攻撃するという処理を実装する
        [SerializeField] private float intervalSeconds = 1.25f;

        private void Start()
        {
            ExecuteAttackSample();
        }

        private void ExecuteAttackSample()
        {
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.D))
                // 一定時間通過拒否
                .ThrottleFirst(TimeSpan.FromSeconds(intervalSeconds))
                .Subscribe(_ => Debug.Log("Attack"));
        }
    }
}
