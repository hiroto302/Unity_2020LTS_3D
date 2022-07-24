using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ToriQiita2
{
    /// <summary>
    /// OnCompletedは「ストリームが完了したためこれ以降メッセージを発行しない」ということを通知するメッセージ。
    /// もしOnCompletedメッセージがSubscribeまで到達した場合、OnError同様にそのストリームの購読は終了し破棄される。
    /// この性質を利用し、ストリームにOnCompletedを適切に発行して上げればまとめて購読終了が実行できるため、
    /// ストリームの後始末をする場合はこのメッセージを発行するようにしよう!!。
    /// なお、一度OnCompletedを発行したSubjectは再利用不可能となります。SubscribeしてもすぐにOnCompletedが返ってくるようになります。
    /// </summary>
    public class OnCompletedSample : MonoBehaviour
    {
        private void Start()
        {
            var subject = new Subject<int>();

            subject.Subscribe(
                n => Debug.Log($"{n}"), 
                () => Debug.Log($"Complete"));
            
            subject.OnNext(1);
            subject.OnCompleted();
            // これ以降は実行されない
            subject.OnNext(2);
            subject.OnCompleted();
        }
    }
}
