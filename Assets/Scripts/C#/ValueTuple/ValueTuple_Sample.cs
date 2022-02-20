using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
C#7.0から、ValueTuple が追加された。複数の要素をグループ化できるデータ構造である。
要素の型づけ と 要素名づけを可能とする
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

        // パターン7
        // ValueTuple の 「分解」 について
        // 分解することで要素を個別に受け取ることができる
        int[] values = { 10, 20, 30, 40, 50};
        // ValueTuple 型で受け取り
        (int Min, int Max) result = CalculateMinMax(values);
        // var 型で受け取り
        var result1 = CalculateMinMax(values);
        // 分解を利用して個別に要素を受け取る
        (int min0, int max0) = CalculateMinMax(values);
        Debug.Log(min0 + " : min0");
        Debug.Log(max0 + " : max0");
        // 分解と var を利用して 個別に受け取ることも可能
        (var min1, var max1) = CalculateMinMax(values);

        // パターン8 の使用
        // out パラメーターを持つ Deconstruct メソッドを拡張関数として使用すれば分解できる
        var (x, y, z) = transform.position;
        Debug.Log(x);

        // パターン9 「破棄」について
        // 分解で受け取る際、使わない変数・要素は警告の対象となる。使わないものを破棄することができる 「 _ 」を利用する。
        (_, _, int age) = LoadPerson();
        Debug.Log(age);
        // このようにすることで、必要な要素のみ受け取れることで、可読性が向上し「使わない変数がある」という警告を回避できる
    }

    // パターン６
    // ValueTuple が活躍する時の一つは、複数の値を返すメソッドを作成する場合
    // 最小値 と 最大値 を同時に返すメソッド
    // わざわざクラスや構造体を作成しなくても、複数の値を返すメソッドを作成することが出来た。
    // しかし、型を作った方が便利な時もある。ValueTuple型は、型の名前がない。名前を持つクラスや構造体を使用することで、コードの可読性が上がるかも。
    (int Min, int Max) CalculateMinMax(int[] values)
    {
        if( values == null || values.Length == 0)
            throw new ArgumentException();

        int min = int.MaxValue;     // int 型で表現できる最大値を 小さい値に更新していく
        int max = int.MinValue;     // int 型で表現できる最小値を 大きい値に更新していく

        foreach(var value in values)
        {
            if (value < min ) min = value;
            if (value > max ) max = value;
        }
        return (min, max);
    }

    // ValueTuple を返すメソッド
    public (string FirstName, string LastName, int Age) LoadPerson()
    {
        return ("T." , "H.", 26);
    }

}

// パターン8
// out パラメーターを持つ Deconstruct メソッドを拡張関数として利用することでも、分解は利用可能になる。
// 以下のように、Unity の Vector3 用の分解のために Deconstruct 拡張関数を定義する
public static class Vector3Extensions
{
    public static void Deconstruct(this Vector3 value, out float x, out float y, out float z)
    {
        // The out parameter 'x' must be assigned to before control leaves the current method
        x = value.x;
        y = value.y;
        z = value.z;
    }
}