using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ToriQita1
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TimeCounter timeCounter;
        [SerializeField] private Text counterText;

        [SerializeField] private TimeCounter_UniRx timeCounterUniRx;
        [SerializeField] private Text counterText_UniRx;
        
        // Start is called before the first frame update
        void Start()
        {
            timeCounter.OnTimeChanged += time =>
            {
                counterText.text = time.ToString();
            };

            timeCounterUniRx.OnTimeChanged.Subscribe(time =>
            {
                counterText_UniRx.text = time.ToString();
            });
        }
    }
}
