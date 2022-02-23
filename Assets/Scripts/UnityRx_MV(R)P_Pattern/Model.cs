using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace UniRxSample.MVRP
{
    // 簡素な Model の実装例
    // データを保持するオブジェクト
    public class Model : MonoBehaviour
    {
        // ReactiveProperty で値を保持
        public readonly IntReactiveProperty Score = new IntReactiveProperty(0);
    }
}
