using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
C#7.0から、ValueTuple が追加された。複数の要素をグループ化できるデータ構造である。
*/
public class ValueTuple_Sample : MonoBehaviour
{

    void Start()
    {
        // 複数の要素をまとめ、アクセスできる
        // 生成パターン 1
        (int Width, int Height) rect = (7, 2);
        Debug.Log(rect.GetType() + " : rect type"); // System.ValueTuple`2[System.Int32,System.Int32]
        Debug.Log(rect.Width);
        Debug.Log(rect.Height);

        // 生成パターン2
        var position = (X: 8, Y: -1, Z: 5);
        // (int X, int Y, int Z) position1 = (8, -1, 5); 上記はこれと同じ
        Debug.Log(position.GetType());  // System.ValueTuple`3[System.Int32,System.Int32,System.Int32]

        // 生成パターン3
        string name = "Hiroto";
        int level = 26;
        var player = (name, level);
        Debug.Log(player);

        // 要素にアクセスする方法 Item1, Item2 .... といった名前で要素にアクセスできる。
        // パターン4
        var tuple = ("ABC", 3, 5.1);
        Debug.Log(tuple);
        Debug.Log(tuple.Item3);

        // ValueTuple における 等値演算子「==」、「!=」の利用
        // パターン5
        var myStatusTuple = (Level: 32, Name: "Kento");
        Debug.Log(myStatusTuple == (Level: 32, Name: "Kento"));     // true  : 完全一致
        Debug.Log(myStatusTuple == (Level: 32, Name: "Kelly"));     // false : 一つでも要素が異なると駄目
        Debug.Log(myStatusTuple == (Level: 32.0f, Name: "Kento"));  // true  : 要素の型が異なっても、暗黙的に変換でき、かつ等値ならばok

        // The tuple element name 'Value' is ignored because a different name or no name is specified on the other side of the tuple == or != operator.
        // 下記については、要素の一つ目が無視されているだけでは? 二つ目の要素だけ比較しているのでは？
        Debug.Log(myStatusTuple == (Value: 32, Name: "Kento"));     // true  : 要素名が違っても、要素の型と順番が同じで、要素が等値ならばok
        Debug.Log(myStatusTuple == (Value: 30, Name: "Kento"));     // false となったので、Value という要素名が無視され、要素の値のみ捉えられている

    }
}
