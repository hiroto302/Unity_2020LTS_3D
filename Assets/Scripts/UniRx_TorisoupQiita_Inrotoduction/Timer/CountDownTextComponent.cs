using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ToriQiitaTimer
{
    public class CountDownTextComponent : MonoBehaviour
    {
        [SerializeField] private RxCountDownTimer rxCountDownTimer;
        [SerializeField] private Text text;

        void Start()
        {
            //タイマの残り時間を描画する
            rxCountDownTimer
                .CountDownObservable
                .Subscribe(time =>
                {
                    //OnNextで時刻の描画
                    text.text = string.Format("残り時間:{0}", time);
                }, () =>
                {
                    //OnCompleteで文字を消す
                    text.text = string.Empty;
                });

            rxCountDownTimer
                .CountDownObservable
                .First(time => time <= 10)
                .Subscribe(_ => text.color = Color.red);

        }
    }
}
