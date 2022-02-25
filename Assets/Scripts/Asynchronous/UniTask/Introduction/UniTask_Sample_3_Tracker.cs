using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


/* UniTask Tracker
現在動いている UniTask をモデリング
UniTask が残っているものがこれで分かる

Window > UniTask Tracker で各項目を有効にすることで確認できる
*/

// UniTask Tracker テスト
public class UniTask_Sample_3_Tracker : MonoBehaviour
{
    async void Start()
    {
        await UniTask.WhenAll(Wait(), Wait(), Wait());
    }
    async UniTask Wait()
    {
        Debug.Log("実行開始");
        // ランダムに 3秒から７秒待機する処理
        await UniTask.Delay(Random.Range(5000, 10000));
    }
}
