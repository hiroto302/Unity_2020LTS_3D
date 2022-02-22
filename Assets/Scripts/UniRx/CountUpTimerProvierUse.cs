using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace UniRxSample
{
    // Timer を使用する側のクラス
    public class CountUpTimerProvierUse : MonoBehaviour
    {
        void Start()
        {
            CountUpTimerProvider timer = GameObject.Find("CountUpTimerProvider").GetComponent<CountUpTimerProvider>();
            timer.TimerCount.Subscribe(time => Debug.Log(time));
        }
    }
}
