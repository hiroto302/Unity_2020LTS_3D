using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Threading.Tasks; // C# 標準機能の Task に必要
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
public class Asynchronous_LoadPrefab : MonoBehaviour
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

    #region  5. async / await を用いて記述したメソッドの実行
        LoadAndInstantiatePrefab("LoadedItem_Sample");
    #endregion

    #region  6. UnitTask を用いて記述したメソッドの実行

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

    /* 5. 4で実装した機能をC#標準機能の async / await で実装する。 ここで、3つのキーワードに着目する

    async : 非同期メソッドであることをマーク. コンパイラに 非同期メソッドとして扱われる
    - 戻り値がないときは、void または Task などを定義。エラーハンドリングを行うために async Task とするのが一般的)
    - 非同期メソッドから値を返す場合は、戻り値の型を Task<T> などにする
    - Unity のイベント処理(Start や Awake、ボタンのクリックなど) を 非同期かできる

    await : 非同期な(async) メソッドの実行完了を待つという宣言。非同期処理内(async キーワードのあるメソッド) のみ利用可能。対象の待ち受けが可能となると言える
    - await 木ワードは、非同期メソッド内で何度も使える
    - コルーチンの yield return に相当
    - 非同期メソッドの結果を受け取って変数に入れられる

    Task : 「非同期処理の実行タイミングの制御や、その結果の保持」の役割
    - 非同期の動作を管理するクラス
    - Task を返さないメソッドは await できない デメリット
    - こいつは、クラスなので Task を生成すると メモリに負荷がかかる デメリット

    「 async / await 」 は、単に 「非同期処理を簡単に待ち受けられるようにする」のが役割である
    「 Task 」 は、「非同期処理の実行」 が役割である

    */

    // 5. 非同期処理(async) 戻り値は GameObject(Task) のメソッド
    async Task<GameObject> LoadPrefab(string name)
    {
        ResourceRequest request = Resources.LoadAsync<GameObject>($"Coroutine_LoadPrefab/{name}");
        // isDone : 動作が終了したか確認
        while (!request.isDone)
        {
            // 得たいゲームオブジェクトのロードが完了するまで待機
            // awaitを使うことでTaskの終了を待つ
            await Task.Delay(100);
        }
        // 返す値
        return request.asset as GameObject;
    }

    // 上記の Load メソッド実行するメソッド
    async void LoadAndInstantiatePrefab(string name)
    {
        // await で 非同期メソッドの結果を受け取って、Prefab変数に入れている
        GameObject prefab = await LoadPrefab(name);
        Instantiate(prefab);

        // 直接代入も可能
        Instantiate(await LoadPrefab(name));
    }

    // 6. UniTask で記述した非同期メソッド
    async UniTask<GameObject> LoadPrefabUniTask(string name)
    {
        // ロード処理が 一行で書ける！！！！！！
        // 本来ならロードを失敗した時の処理などが入るので ますます恩恵が増えてくる
        // Unity 標準機能だと、下記のような await したい処理(LoadAsync など)が await できない
        // UniTask ならそれが可能となる
        return await Resources.LoadAsync<GameObject>($"Coroutine_LoadPrefab/{name}") as GameObject;
    }

    // 上記の メソッドを実行して、得た返り値から Prefab を生成するメソッド
    async void LoadAndInstantiatePrefabUniTask(string name)
    {
        GameObject prefab = await LoadPrefabUniTask(name);
        Instantiate(prefab);
    }

}