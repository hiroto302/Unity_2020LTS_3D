using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 明示的なインターフェースの実装
同じ名称のメソッドが Interface に実装されているとする.
今回で言えば, Hoge()

*/

public interface IA
{
    void Hoge();
}
public interface IB
{
    void Hoge();
}

public class Explicit : MonoBehaviour, IA, IB
{
    // インターフェース A・B どちらから呼ばれても同じ挙動をさせたい場合
    public void Hoge() { Debug.Log("Hoge A・B");}

    // 明示的な インターフェースの実装
    // インターフェース毎に 挙動を分けたい場合
    // インターフェース経由でしか 呼ぶことが出来ないので public 修飾子 付与する必要がない
    // public でないことが カプセル化の強化につながる
    void IA.Hoge() { Debug.Log("Hoge A");}
    void IB.Hoge() { Debug.Log("Hoge B");}

    void Start()
    {
        Hoge();

        // Interface A にキャストしてから実行
        IA a = this;
        a.Hoge();

        // Interface B にキャストしてから実行
        IB b = this;
        b.Hoge();
    }
}
