using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ローカル関数について
C#7.0より「あるメソッドからだけ呼び出すことが可能なメソッド」を作成を作成すること可能
メソッドの中に、メソッドを定義できる機能
コードの見通しが良くなる。
ローカル関数の使用例に、Coroutine をみる
*/
public class LocalFunction : MonoBehaviour
{
    // 例１
    public Coroutine Launch()
    {
        return StartCoroutine(LaunchImpl());

        // 下記のコルーチンは、Launch() メソッドからのみ呼び出せれることが一目で分かる
        IEnumerator LaunchImpl()
        {
            yield break;
        }
    }

    // 例２ 外側のメソッドの変数をキャプチャできる
    public Coroutine Launch1()
    {
        string message = " hello local function1";
        return StartCoroutine(LaunchImpl());

        IEnumerator LaunchImpl()
        {
            // 外側のメソッドの変数messageをキャプチャ
            // ローカル関数内部でも利用可能
            Debug.Log(message);
            yield break;
        }
    }

    // 例３ 上記は便利なこともあるが、意図しないキャプチャをしてしまう可能性がある。
    // C# 8.0 から静的ローカル関数が追加された。これにより外側のメソッドの変数をキャプチャしないことを防ぐことができる。
    public Coroutine Launch2()
    {
        string message = " hello local function2";
        Debug.Log(message);
        return StartCoroutine(LaunchImpl());

        // 静的ローカル変数
        static IEnumerator LaunchImpl()
        {
            // 外側のメソッドの変数messageをキャプチャできない
            // Debug.Log(message);
            yield break;
        }
    }
}
