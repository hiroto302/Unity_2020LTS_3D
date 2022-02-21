using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* out 変数について
out パラメータ修飾子 を使用した参照渡しメソッドは、次のような特徴を持つ
・返り値型は bool . 処理が成功した時は、true 失敗した時は false
・処理が成功した場合、out パラメータ修飾子がついたパラメータ の参照渡しで結果を返す

具体例として、int.TryParse・Physics.Raycast、 Component.TryGetComponent などがあげられる
C# 6.0 までは、out パラメータ修飾子を利用した参照渡しのメソッドを利用する場合、メソッドより前に宣言する必要があった
C# 7.0 から 加わった out 変数により、メソッド呼び出し箇所で変数宣言ができるようになり、簡潔に宣言できるようになった。
*/
public class Out_Sample : MonoBehaviour
{
    void Start()
    {
        TryParse();
        TryGetComponent();
    }

    void TryParse()
    {
        string numberString = "1";
        string numberString2 = "2";
        // out 修飾子を付与する パラメータをここで宣言
        int number;
        // int.TryParse : 文字列を数値に変換できるかチェック
        if (int.TryParse(numberString, out number))
        {
            Debug.Log(number + " : numberString は 整数文字列に変換可能");
        }

        // out 変数の利用
        if (int.TryParse(numberString2, out int number2))
        {
            Debug.Log($"numberString2は、{number2} である");
        }
    }

    void TryGetComponent()
    {
        Rigidbody rb;
        // 対象のオブジェクトが rigidbody コンポーネントを持っているか
        if( TryGetComponent(out rb))
        {
            // 略
        }
        else
        {
            Debug.Log("ないよ");
        }
    }

}
