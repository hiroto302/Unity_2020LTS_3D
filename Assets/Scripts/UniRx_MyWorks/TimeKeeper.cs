using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTimerImpl
{
    /*  REMARK : 実装したいものメモ
     *  GameStateView の State が Answer になったら、6秒経過を計測開始する
     *  6秒の間に Player は カードをタップすることができる
     *  - 6秒以内にタップした時 (GameState が Answer => Judge になった時)
     *  計測を停止する。
     *  - 6秒以内にタップすることが出来なかった時 (GameState は Answer のまま)
     *  - 6秒経過したとことを知らせる。GameStateView に??
     */
    public class TimeKeeper : MonoBehaviour
    {
        
    }
}
    
