using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
/* そのほかのメッセージ発行方法 : ReactiveProperty シリーズ
ReactiveProperty<T> は、「Subject<T> の機能が同梱された変数」として振る舞う。
変数と同じように、値の読み書きが可能だが、その値の変動を OnNext メッセージとして外部に伝えることができる。
なお、ReactiveProperty<T>は、「値が変更した場合」に OnNextメッセージを発行する。
書き込んだ値が、直前の値と同値であった場合は、メッセージ発行されない
*/
public class ReactivePropertySample : MonoBehaviour
{
    void Start()
    {
        // int 型の ReactiveProperty
        var rp = new ReactiveProperty<int>(0);

        // 値が変動した時、ログを出す
        rp.Subscribe(x => Debug.Log(x));

        // ReactiveProperty の中身を書き換え
        rp.Value = 1;
        rp.Value = 2;
        rp.Value = 2;
        rp.Value = 3;

        // 破棄
        rp.Dispose();
    }

}
