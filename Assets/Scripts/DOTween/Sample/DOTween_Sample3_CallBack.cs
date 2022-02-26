using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/* コールバック
DOTweenは実行時に各種タイミングでコールバックを呼び出すことが可能
代表的なものを記述する
*/
public class DOTween_Sample3_CallBack : MonoBehaviour
{
    void Start()
    {
        // Example();
        // Example2();
        // Example3();
        // Example4();
        // Example5();
        Example6();
    }

    private void Update()
{
    if(Input.GetKeyDown(KeyCode.K))
    {
        this.transform.DOKill();
    }
}


    // 1. 「OnComplete」 : 完了時のコールバック
    void Example()
    {
        // ラムダ式
        this.transform.DOMoveX(3.0f, 2.0f).OnComplete(() => Debug.Log("実行完了"));
        // or
        // this.transform.DOMoveX(3.0f, 2.0f).OnComplete(CallBackMethod);
    }
    void CallBackMethod()
    {
        Debug.Log("実行完了");
    }

    void Example2()
    {
        // 動作をつないでいくようなことも出来る
        this.transform.DOMove(new Vector3(5f, 0f, 0f), 2f).OnComplete(() =>
        {
            this.transform.DORotate(Vector3.forward * 180f, 1f);
        });
    }

    // 2. 「OnStart」: 実行開始時のコールバック
    //     SetDelayなどを使って遅延させた場合は遅延終了後にOnStartが呼ばれる
    void Example3()
    {
        transform.DOMoveX(3.0f, 2.0f)
            .SetDelay(2.0f)
            .OnStart( () => Debug.Log($"実行開始 : {Time.realtimeSinceStartup}"))
            .OnComplete( () => Debug.Log($"実行完了 : {Time.realtimeSinceStartup}"));
    }

    // 3. 「OnUpdate」 : 実行中のコールバック。実行中に毎フレーム呼ばれる
    void Example4()
    {
        transform.DOMoveX(3.0f, 2.0f).OnUpdate(() =>
            Debug.Log($"[{Time.frameCount}] OnUpdate {this.transform.position}") );
    }

    // 4. 「OnKill」: 停止時のコールバック
    void Example5()
    {
        this.transform.DOMove(new Vector3(5, 0, 0), 3.0f)
            .SetLoops(3, LoopType.Yoyo)
            .OnKill(() => Debug.Log("OnKill"+transform.position));
    }

    // 下記のようにOnKillとOnComplete両方を登録した場合移動完了時に両方のコールバックが呼び出される
    // 途中でKillした場合はOnKillのみが呼び出される。なお移動完了時もOnKillは呼び出される
    void Example6()
    {
        this.transform.DOMove(new Vector3(5f, 0f, 0f), 1f)
        .OnKill(() =>
        {
            Debug.Log("OnKill"+this.transform.position);
        })
        .OnComplete(()=>
        {
            Debug.Log("OnComplete" + this.transform.position);
        });
    }
}
