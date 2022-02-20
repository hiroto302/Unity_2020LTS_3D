using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 構造体は、別の変数へ代入する時や引数として渡す時、そして返り値として返す時はコピーが発生する
// 参照型との違いを意識しながら実装していくこと
namespace Structure
{
    public class Struct_Sample : MonoBehaviour
    {
        int num = -1;
        int num1 = 3;
        void Start()
        {
            // ReadOnlyExample readOnlyExample = new ReadOnlyExample();
            // readOnlyExample.UpdateStructExample();

            Example1 refExample1 = new Example1();
            refExample1.Execution();
            // ref は、代入可能な変数であること
            ref int refNum =  ref refExample1.RefExample(ref num1, 2);
            Debug.Log(refNum);
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

            /* 1.防御的コピーについて
            「 readonly 修飾子がついている構造体」の中身の書き換えを防ぐための仕組み
            防御的コピーにより、readonly修飾子がついている構造体のフィールドのメソッド呼び出しでは、
            そのフィールドのオブジェクトのコピーが発生する。そして、コピーしたオブジェクトがメソッドを呼び出す。
            */
            Debug.Log(readOnlyField); // 1
            readOnlyField.Update(5);  // 防御的コピーが実行され、コピーされた構造体が５に更新される
            Debug.Log(readOnlyField); // 1
        }
    }

    /* 2.参照戻り値と参照ローカル変数について
        構造体のコピーが発生した時、データの構造が大きい場合は特に、パフォーマンス上の問題となる
        それを回避する方法について考えていく
    */
    public class Example1
    {
        // 参照引数 ref パラメーターを使用し、参照渡しを行うことで、
        // メソッドに引数を渡す時のオブジェクトコピーの抑制が可能
        // しかし、下記のままだと 「メソッドが返り値を返す時のオブジェクトのコピー」が発生してしまう
        public int Max(ref int num, ref int num1)
        {
            if(num > num1){
                return num;
            }else
                return num1;
        }
        // C#7.2 から、参照戻り値(ref戻り値) と 参照ローカル変数(refローカル変数)が使用可能になった
        // 下記では、参照戻り値を返すメソッドを実装。これにより「メソッドが返り値を返す時のオブジェクトのコピー」が発生しない
        public ref int Max1(ref int num, ref int num1)
        {
            if(num > num1){
                return ref num;
            }else
                return ref num1;
        }

        int num = -1;
        int num1 = 3;
        public void Execution()
        {
            // 上記の参照戻り値 Max1 を受け取るには、参照ローカル変数で受け取る
            // 下記のように記述することで max は参照ローカル変数となる
            ref int max = ref Max1(ref num, ref num1);
            Debug.Log(max + " : 参照ローカル変数");
            // var も使える
            ref var max1 = ref Max1(ref num, ref num1);
            // 下記の記述では、参照戻り値にならないことに注意
            // int max2 = Max1(ref num, ref num1);

            ref int refnum = ref RefExample(ref num, 2);
        }

        // 参照戻り値は、どんな変数でも参照として返り値を返せるわけではないことに注意すること
        // その型のフィールドや引数として受け取った参照引数(ref パラメーター)などは、参照戻り値として返すことが可能。
        // 参照引数を渡す時は、代入可能な変数を渡すこと (定数とか駄目)
        // なので、ローカル変数とかは返せない
        public ref int RefExample(ref int refNum, int num)
        {
            int localValue = 1;

            // 下記は、ローカル変数なのでコンパイルエラー
            // return localValue;
            // 参照引数ではないので駄目
            // return ref num;

            // これは ok
            refNum = localValue;
            return ref refNum;
        }
    }
}
