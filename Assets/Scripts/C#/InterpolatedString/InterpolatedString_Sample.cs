using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 文字列補完について
1. string.Format
2. Debug.LogFormat
3. C#6.0 より 「$」を文字列の先頭につけることで文字列補完が可能になった
4. c#8.0 以降 「$@」を文字列の先頭につけることで、複数行(段落がつく)の文字列補完が可能
5. Format と同様に、0埋めや行数などの文字列補完も可能
*/
public class InterpolatedString_Sample : MonoBehaviour
{
    string playerName = "Hiroto";
    int level = 26;
    string displayInfo;
    string multiLineMessage;
    void Start()
    {
        // 1. 生成する文字列の中に、変数が参照するオブジェクトや値を生成するときは、 stringFormat を利用していた
        displayInfo = string.Format("PlayerName: {0}, Level: {1}", playerName, level);
        Debug.Log(displayInfo);

        // 2. ログ目的の場合
        Debug.LogFormat("PlayerName: {0}, Level: {1}", playerName, level);

        // 3. 「$」を利用した文字列補完 : 補完したい箇所に実際の変数を記述することが可能となった
        // これにより、より簡潔に分かりやすい文字列の生成が可能となった
        displayInfo = $"PlayerName: {playerName}, Level: {level}";
        Debug.Log(displayInfo);

        // 4. 複数行の文字列補完
        multiLineMessage = $@"Hello! {playerName}! 
        Your level is {level} !";
        Debug.Log(multiLineMessage);

        // 5. 3桁確保の文字列補完
        displayInfo = $"Level : {level:000}";
        Debug.Log(displayInfo);
    }

}
