using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using Cysharp.Threading.Tasks;

/* 同期処理と非同期処理の違い をみる
同期処理 : 書いた通りに順番に処理が実行されていく
非同期処理 : 実行中の処理の完了を待たずに、次の処理を実行する処理方式 これを念頭にすること

下記のサンプルでは、ある一定時間経過後 に ロードしたオブジェクトを生成するものを実装した

同期で書いたものは、一定時間経過後 オブジェクトを生成するために スレッドを停止させてしまうので
並列して複数の処理を実行することができい

しかし、非同期で実装することで、並行して行いたい処理を止めることなく オブジェクトを生成することができる

勘違いしてはいけないのは、Unity のコルーチンは、メインスレッドで実行される非同期処理の機構である。
処理を細切れに毎フレーム 呼び出すことで、CPUが同時に実行する処理は１つながらも、擬似的に複数処理を実行しているように見せている
*/
public class Coroutine_LoadPrefab : MonoBehaviour
{

    Action<GameObject> InstantiatePrefabAction;
    void Start()
    {
    #region 1.同期で記述したもの
        // // Stopwatch インスタンス作成
        // var sw = new System.Diagnostics.Stopwatch();
        // // Stopwatch 開始
        // sw.Start();
        // DelayedInstantiateItem("LoadedItem_Sample", 1.0f, sw);
        // Debug.Log($"Item 1 生成実行後 : {sw.ElapsedMilliseconds}ミリ秒");
        // DelayedInstantiateItem("LoadedItem_Sample", 1.0f, sw);
        // Debug.Log($"Item 2 生成実行後 : {sw.ElapsedMilliseconds}ミリ秒");
    #endregion

    #region 2.非同期(コルーチン)で記述したもの => async にあたる概念
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        StartCoroutine(DelayedInstantiateItemRoutine("LoadedItem_Sample", 1.0f, sw));
        DelayedInstantiateItem("LoadedItem_Sample", 1.0f, sw);
        StartCoroutine(DelayedInstantiateItemRoutine("LoadedItem_Sample", 1.0f, sw));
        DelayedInstantiateItem("LoadedItem_Sample", 1.0f, sw);
    #endregion

    #region 3. コルーチン (同期) => await にあたる概念
        StartCoroutine(StartRoutine());
    #endregion

    #region  4.コルーチンのデメリット
        // LoadPrefabRoutineコルチーン処理の中で得た結果である、
        // GameObject 型の返り値をもとに、Instantiate メソッドを実行
        StartCoroutine(LoadPrefabRoutine("LoadedItem_Sample", (prefab) => Instantiate(prefab)));
        // 自作メソッド を 上記のようなラムダ式で記述
        StartCoroutine(LoadPrefabRoutine("LoadedItem_Sample", (prefab) => InstantiateLoadedPrefab(prefab)));
        // 上記の方法を知らなければ下記のような式になることが考えられる
        // コルーチン内で、得た結果(返り値)から指定したメソッド実行したいとき、記述が長くなる
        // なら、コルーチンの中に引数で指定するメソッドではなく、直接書けばいいとなるかもしれないけど
        // それは、再利用性の無いコルーチンになってしまう
        // また、不雑で追いにくい & ネストが深くなる(実行するコルチーンの内容をみて、さらに引数で与えたメソッドについても見なければならない。2つの箇所をみる必要がある)
        // さらには、Action(デリゲート)・ラムダ式は GCAlloc の発生になってしまうこともあるから、パフォーマンス的にも微妙
        InstantiatePrefabAction += InstantiateLoadedPrefab;
        StartCoroutine(LoadPrefabRoutine("LoadedItem_Sample", InstantiatePrefabAction));
    #endregion
    }

    // nameで 指定した prefab を seconds 送らせて生成するメソッド
    void DelayedInstantiateItem(string name, float seconds, System.Diagnostics.Stopwatch sw)
    {
        // Resources フォルダ内の path に格納されている、要求されたタイプのアセットを読み込む
        // なければ null を返す
        // Resources 以下のバスを引数に記述
        GameObject prefab = Resources.Load<GameObject>($"Coroutine_LoadPrefab/{name}");
        // 指定したミリ秒数の間現在のスレッドを中断
        Thread.Sleep((int)seconds * 100);
        Instantiate(prefab);
        Debug.Log($"{name}生成完了 : {sw.ElapsedMilliseconds}ミリ秒");
    }

    // 非同期で利用したコルーチン
    IEnumerator DelayedInstantiateItemRoutine(string name, float seconds, System.Diagnostics.Stopwatch sw)
    {
        GameObject prefab = Resources.Load<GameObject>($"Coroutine_LoadPrefab/{name}");
        // 指定した秒数経過後、下記に続く処理を実行 (非同期で指定した時間待つ)
        // 次に続く処理の実行を待ち受けるもの（イベント処理の待ち受け)
        yield return new WaitForSeconds(seconds);
        Instantiate(prefab);
        Debug.Log($"{name}生成完了 : {sw.ElapsedMilliseconds}ミリ秒");
    }

    // 3. コルーチン (同期) => await にあたる概念
    // (コルーチンの中で)コルーチンの完了を待ってから次の処理を実行していく処理
    // コルーチンを同期的に実行していくもの
    IEnumerator StartRoutine()
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        // 指定したコルーチンが完了後、下記に続く処理を実行
        yield return DelayedInstantiateItemRoutine("LoadedItem_Sample", 1.0f, sw);
        DelayedInstantiateItem("LoadedItem_Sample", 1.0f, sw);
        yield return DelayedInstantiateItemRoutine("LoadedItem_Sample", 1.0f, sw);
        DelayedInstantiateItem("LoadedItem_Sample", 1.0f, sw);
    }

    // 4. コルーチン内で実行された処理の結果を返すための コルーチンの記述
    // どうしても長くなる
    // 得たい結果の値に合う型の Action を引数に指定
    IEnumerator LoadPrefabRoutine(string name, System.Action<GameObject> onLoaded)
    {
        ResourceRequest request = Resources.LoadAsync<GameObject>($"Coroutine_LoadPrefab/{name}");
        // 得たいゲームオブジェクトのロードが完了するまで待機
        // ResourceRequestは、 AsyncOperation(非同期動作のコルーチンで使用) なので下記のような記述ができる
        yield return request;
        // 返り値が GameObject のメソッドを実行
        // asset : 読み込まれているアセットバンドルを返す
        onLoaded(request.asset as GameObject);
    }

    // 上記のコルーチンの結果をもとに実行するメソッド
    void InstantiateLoadedPrefab(GameObject prefab)
    {
        Instantiate(prefab);
    }
}