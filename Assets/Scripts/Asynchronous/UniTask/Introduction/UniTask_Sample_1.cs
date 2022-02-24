using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

// UniTask.WhenAll を利用した 変数受け取り
// Task の中でも Unity で便利なものは残っている
public class UniTask_Sample_1 : MonoBehaviour
{
    async UniTask Start()
    {
        // Tuple で Load した GameObject変数の受け取る
        var (item1, item2, item3) = await UniTask.WhenAll(
            LoadPrefab("Item1"),
            LoadPrefab("Item2"),
            LoadPrefab("Item3")
            );

        // 受けとった オブジェクトを生成
        Instantiate(item1);
        Instantiate(item2);
        Instantiate(item3);
    }

    // 引数に指定した name のオブジェクトを Load して 返す メソッド
    async UniTask<GameObject> LoadPrefab(string name)
    {
        return await Resources.LoadAsync(name) as GameObject;
    }
}
