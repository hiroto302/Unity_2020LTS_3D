using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToriQita1
{
    public class TimerCounter : MonoBehaviour
    {
        // イベントハンドラー(イベントメッセージの型定義)
        public delegate void TimerEventHandler(int time);
        // event
        public event TimerEventHandler OnTimeChanged;

        private void Start()
        {
            StartCoroutine(TimerCoroutine());
        }

        IEnumerator TimerCoroutine()
        {
            var time = 100;
            while (time > 0)
            {
                time--;
                OnTimeChanged(time);
                yield return new WaitForSeconds(1);
            }
        }
    }
}
