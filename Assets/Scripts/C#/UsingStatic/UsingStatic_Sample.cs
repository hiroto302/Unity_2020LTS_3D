using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* using static ディレクティブについて
sing static ディレクティブを使用することで、型名を書かずに対象の型の静的メンバーを記述できるようになる
Mathf型 と 他の名前空間にある enum型 を例に記す

enumの型名を Sate など 他の名前空間・クラスで 同名になるものが想定される時は、使用しない
使い所を考慮する
switch 文では 渡す変数名の所で、名前空間や クラス・型名をきちんと記述すること判別できるようになどできるが意味が無くなる
*/

using static UnityEngine.Mathf;

using ExampleNamespace;
using static ExampleNamespace.Orientation;

using static EnumSampleClass.State;
using static EnumSampleClass2.State;

public class UsingStatic_Sample : MonoBehaviour
{
    public float CircleArea(float radius)
    {
        // 下記のように、UnityEngin.Mathf型の静的メンバーである PI を、型名なしで記述が可能
        return radius * radius * PI;
    }

    // using Static ディレクティブを使用しないと、冗長になる
    public Vector2 ExampleNamespaseOrientationToVector2(ExampleNamespace.Orientation orientation)
    {
        switch(orientation)
        {
            case ExampleNamespace.Orientation.Up : return Vector2.up;
            default : throw new System.InvalidOperationException();
        }
    }

    // 使用した記述
    public Vector2 OrientationToVector2(Orientation orientation)
    {
        switch(orientation)
        {
            case Up : return Vector2.up;
            default : throw new System.InvalidOperationException();
        }
    }

    // 同名の名前空間で他のクラスの enum を扱う場合
    void ReferenceEnumSampleClassState(EnumSampleClass.State state)
    {
        switch(state)
        {
            case EnumSampleClass.State.Normal : Debug.Log("Normal"); break;
            // using static ディレクティブに、同名の型名を持つクラスがあるので判別できなくなる
            // case Abnormal: Debug.Log(Abnormal); break;
            default : throw new System.InvalidOperationException();
        }
    }
}

namespace ExampleNamespace
{
    public enum Orientation
    {
        Up,
        Right,
        Down,
        Left
    }
}

public class EnumSampleClass
{
    public enum State
    {
        Normal,
        Abnormal
    }
}
public class EnumSampleClass2
{
    public enum State
    {
        Normal,
        Abnormal
    }
}
