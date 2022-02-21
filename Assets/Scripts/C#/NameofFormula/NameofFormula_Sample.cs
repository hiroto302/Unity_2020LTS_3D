using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* nameof 式について
変数名・型名・メソッド・フィールド・プロパティーなどの名称を、文字列として取得できる式である。
これらの名称を文字列として扱いたい場合有効である
*/
namespace NameofFormula
{
    // Shooter クラスの shoot メソッドを呼び出すクラス(Controllerクラス)
    public class NameofFormula_Sample : MonoBehaviour
    {
        [SerializeField] Shooter shooter = null;
        void Start()
        {
            // GameObject.SendMessage : 他のクラウスのメソッドを呼び出す処理
            // 下記のような、引数に文字列を必要とするメソッドがあるとする。
            // shooter 側で メソッド名が変更された場合、実行するまで不具合に気づくことができない恐れがある
            shooter.SendMessage("Shoot");
            // このような問題に対応するために、nameof式を利用する
            shooter.SendMessage(nameof(Shooter.Shoot));
            // もし、Shooter側の Shoot メソッド名が変更された場合、実行時の不具合ではなく、コンパイルエラーとなるので
            // 問題にすぐ気づける
            // 対応する箇所の変更忘れを防ぐことができる。
        }
    }
}
