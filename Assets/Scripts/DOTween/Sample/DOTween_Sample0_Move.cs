using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  // DOTween を使うためにいる

public class DOTween_Sample0_Move : MonoBehaviour
{

    void Start()
    {
        // Move();
        // MoveLoop();
        // MoveDelay();
        // MoveEase();
        MoveMix();
    }
    // 1. DOMove
    void Move()
    {
        // 3秒かけて (5, 0, 0) へ移動する
        this.transform.DOMove(new Vector3(5, 0, 0), 3.0f);
    }

    // 2. SetLoops : 動作を繰り返す。繰り返す回数(loops)と繰り返し方法(loopType)を設定。loopsに-1を入れるとずっと繰り返す挙動になる
    // LoopType
    // Yoyo : 移動後、最初の位置に戻るように動く
    // Restart : 移動後、最初の位置から再び移動を開始
    // Increment : 移動後、その位置から再び移動を開始
    void MoveLoop()
    {
         // (5,0,0)へ1秒で移動するのを3回繰り返す
        // this.transform.DOMove(new Vector3(5, 0, 0), 3.0f).SetLoops(3, LoopType.Yoyo);
        // this.transform.DOMove(new Vector3(5, 0, 0), 3.0f).SetLoops(3, LoopType.Restart);
        this.transform.DOMove(new Vector3(5, 0, 0), 3.0f).SetLoops(-1, LoopType.Incremental);
    }

    // 3. SetDelay : 動作が開始するのを遅延
    void MoveDelay()
    {
        this.transform.DOMove(new Vector3(5, 0, 0), 3.0f).SetDelay(2.0f);
    }

    // 4. Ease : SetEaseを使い、始点と終点をどのように繋ぐか設定する
    // Easeには様々な種類がある。使う際はいろいろなパラメータを変えながら、作りたい動きを探そう!
    // SetEase を使わない場合は OutQuad がデフォルトで設定
    void MoveEase()
    {
        this.transform.DOMove(new Vector3(5, 0, 0), 3.0f).SetEase(Ease.Linear);
    }

    // 5. 組み合わせる
    void MoveMix()
    {
            //2秒待ってから(5,0,0)へ3秒で移動するのを4回(2往復) OutBounceで行う
            this.transform.DOMove(new Vector3(5f, 0f, 0f), 3f)
                    .SetDelay(2f)
                    .SetLoops(4, LoopType.Yoyo)
                    .SetEase(Ease.OutBounce);
    }

}
