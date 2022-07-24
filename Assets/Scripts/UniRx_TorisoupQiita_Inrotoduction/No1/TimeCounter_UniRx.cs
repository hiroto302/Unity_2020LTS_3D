using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ToriQita1
{
    public class TimeCounter_UniRx : MonoBehaviour
    {
        private Subject<int> timerSubject = new Subject<int>();
        public IObservable<int> OnTimeChanged => timerSubject;

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
                timerSubject.OnNext(time);
                yield return new WaitForSeconds(1);
            }
        }
    }
}
