using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/* 止める Kill
DOTweenの実行を止める方法を4つ紹介。 シチュエーションに合った止め方を選択できるようにする
Killするとオブジェクトは移動中であってもその場で停止するなど, 行なっている処理を中断させる事ができる
*/
public class DOTween_Sample1_Kill : MonoBehaviour
{
    // 実行する Tween 変数
    Tween tween;

    void Start()
    {
        tween = Move();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            // 1. 返り値 を保存しておいて止める方法
            tween.Kill();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            // 2. 参照元 を指定して止める方法
            this.transform.DOKill();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            // 3. Object を指定して止める方法
            DOTween.Kill(this.transform);
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            // 4. 全て の実行を止める方法. (このスクリプトコンポーネント 以外で実行されている object 以外のものも含めて全て止める。)
            DOTween.KillAll();
        }
    }

    // DOTween を利用するメソッド
    Tween Move()
    {
        return transform.DOMove( new Vector3(0, 5, 0), 3.0f).SetLoops(-1, LoopType.Yoyo);
    }
}
