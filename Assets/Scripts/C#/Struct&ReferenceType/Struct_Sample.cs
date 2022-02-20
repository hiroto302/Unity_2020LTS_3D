using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 構造体は、別の変数へ代入する時や引数として渡す時、そして返り値として返す時はコピーが発生する
// 参照型との違いを意識しながら実装していくこと
namespace Structure
{
    public class Struct_Sample : MonoBehaviour
    {
        void Start()
        {
            ReadOnlyExample readOnlyExample = new ReadOnlyExample();
            readOnlyExample.UpdateStructExample();
        }
    }

    // readonly 修飾子について
    // 構造体において、readonly フィールドでは 防御的コピーが生じることがある。
    // 構造体の StructExample を作成
    public struct StructExample
    {
        int value;
        public StructExample(int initialValue)
        {
            value = initialValue;
        }
        public void Update(int updateValue)
        {
            value = updateValue;
            Debug.Log(value + " : 呼び出す構造体メソッド内のフィールド");
        }
        public override string ToString()
        {
            return value.ToString();
        }
    }
    public class ReadOnlyExample
    {
        // 普通のフィールド
        public StructExample normalField = new StructExample(1);
        // readonly フィールド
        public readonly StructExample readOnlyField = new StructExample(1);

        public void UpdateStructExample()
        {
            // 普通のフィールド
            // Debug.Log(normalField); // 1
            // normalField.Update(5);  // 値が更新される
            // Debug.Log(normalField); // 5

            /* 防御的コピーについて
            「 readonly 修飾子がついている構造体」の中身の書き換えを防ぐための仕組み
            防御的コピーにより、readonly修飾子がついている構造体のフィールドのメソッド呼び出しでは、
            そのフィールドのオブジェクトのコピーが発生する。そして、コピーしたオブジェクトがメソッドを呼び出す。
            */
            Debug.Log(readOnlyField); // 1
            readOnlyField.Update(5);  // 防御的コピーが実行され、コピーされた構造体が５に更新される
            Debug.Log(readOnlyField); // 1
        }
    }
}
