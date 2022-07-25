using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ToriQiitaTimer
{
    public class RxCountDownTimer : MonoBehaviour
    {
        // IConnectableObservable<T> : なんだこれ
        private IConnectableObservable<int> _countDownObservable;

        /// カウントダウンストリーム 。このObservableを各クラスがSubscribeする
        public IObservable<int> CountDownObservable => _countDownObservable;

        public bool isGame = true;

        private void Awake()
        {
            //60秒カウントのストリームを作成
            //PublishでHot変換
            _countDownObservable = CreateCountDownObservable(20).Publish();
        }

        private void Start()
        {
            // Start時にカウント開始
            _countDownObservable.Connect();
            
            // ゲームの状態が変化させる
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.A))
                .Subscribe(_ => isGame = false);
        }
        
        /// <summary>
        /// CountTimeだけカウントダウンするストリーム
        /// </summary>
        /// <param name="countTime"></param>
        /// <returns></returns>
        
        // REMARK : ただし、このCreateCountDownObservableで作ったストリームはColdであるという点に注意
        //（Subscribeしたタイミングで始めてカウントダウンが開始される点、1つのタイマストリームを複数SubscribeするにはHot変換が必要である点）
        // Rxのストリームは基本的にSubscribeされた瞬間に各オペレータの動作が始まる
        private IObservable<int> CreateCountDownObservable(int countTime)
        {
            return Observable
                //0秒後から1秒間隔で実行
                .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
                // long型 : 64ビット整数 、 int型 : 32ビット整数
                // xは起動してからの秒数
                .Select(x => (int) (countTime - x))
                // 0秒超過の間はOnNext、0になったらOnComplete 。 isGame が false になると InvalidOperationException: sequence is empty  エラーになってまう
                // TakeWhile : 
                .TakeWhile(x => x > 0 && isGame);
        }
    }
}

