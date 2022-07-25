using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ToriQiitaTimer
{
    public class GameTimerManager : MonoBehaviour
    {
        /// <summary>
        /// 試合開始前のカウントダウン
        /// </summary>
        public IObservable<int> GameStartCountDownObservable{get;private set;}

        /// <summary>
        /// 試合中のカウントダウン
        /// </summary>
        public IObservable<int> BattleCountDownObservable { get; private set; }
        
        // Start is called before the first frame update
        void Start()
        {
            //試合前の3秒タイマ
            //3秒タイマのストリームをPublishでHot変換（まだConnectはしない）
            var startConnectableObservable = CreateCountDownObservable(3).Publish();
            //外に公開するためのObservableとして保存
            GameStartCountDownObservable = startConnectableObservable;

            //試合中の20秒タイマ
            //20秒タイマのストリームをPublishでHot変換（まだConnectはしない）
            var battleConnectableObservable = CreateCountDownObservable(20).Publish();
            BattleCountDownObservable = battleConnectableObservable;

            // 3秒タイマのOnCompleteで60秒タイマをConnectする（60秒タイマの起動）
            GameStartCountDownObservable.Subscribe(_ => { ; }, () => battleConnectableObservable.Connect());
            
            // 60秒タイマの後ろにConcatで5秒タイマを連結し、そのOnCompleteでシーン遷移させる
            BattleCountDownObservable
                .Concat(CreateCountDownObservable(5))
                .Subscribe(_ => { ; }, () => { Application.LoadLevel("NextScene"); });

            //3秒タイマ起動
            startConnectableObservable.Connect();
        }
        
        /// <summary>
        /// CountTimeだけカウントダウンするストリーム
        /// </summary>
        /// <param name="CountTime"></param>
        /// <returns></returns>
        private IObservable<int> CreateCountDownObservable(int CountTime)
        {
            return Observable
                .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1)) 
                .Select(x => (int)(CountTime - x))                       
                .TakeWhile(x => x > 0);                                 
        }
    }
}
