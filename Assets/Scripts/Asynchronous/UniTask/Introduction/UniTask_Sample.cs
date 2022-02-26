using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // これがいる , 昔は UniRx.Async だった

/*
UniTaskとはUnity向けに最適化されたTask実装を提供するライブラリ。
もとはUniRxに組み込まれていましたが途中で分離し、独立した1つのライブラリとして提供されるようになった。
*/

public class UniTask_Sample : MonoBehaviour
{
        async UniTask Start()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            // 約3000 ミリ秒で終わる
            await Example();
            await Example();
            await Example();
            Debug.Log($"{sw.ElapsedMilliseconds} : 3つのTask処理完了");

            // 約 1000 ミリ秒で終わる
            // WhenAll : 引数に与えた UniTask の処理全てが終了したら await を抜ける
            await UniTask.WhenAll(Example(), Example(), Example());
            Debug.Log($"{sw.ElapsedMilliseconds} : 同時に3つのTask処理完了");
        }

        async UniTask Example()
        {
            // 1000 ミリ秒で終わる UniTask
            await UniTask.Delay(1000);
        }
}
