using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ToriQita1
{
    public class DisplayMsg : MonoBehaviour
    {
        private Subject<string> msgSubject = new Subject<string>();

        private void Start()
        {
            msgSubject.Subscribe(msg => Debug.Log($"1, {msg}"));
            
            msgSubject.Subscribe(
                m => Debug.Log($"2, {m}"),
                () => Debug.Log($"2, Complete"));
            
            msgSubject.OnNext("Hello World");
            msgSubject.OnCompleted();
            
            // Complete が発行されて、OnCompleted() が発行されているので　下記は実行されない。
            msgSubject.OnNext("ハロー ワールド");
            
            // ShowMsg("Hello !!");
        }

        void ShowMsg(string msg)
        {
            Subject<string> subject = new Subject<string>();
            
            // IObserver には、OnCompleted, OnError, OnNext が実装されている
            
            // OnNext のみ
            subject.Subscribe(m => Debug.Log($"1, {m} + OnNext only"));
            
            // OnNext & OnError
            subject.Subscribe(
                m => Debug.Log($"2, {m}"),
                error => Debug.Log($"{error} : Error"));
            
            // OnNext & OnCompleted
            subject.Subscribe(
                m => Debug.Log($"3, {m}"),
                () => Debug.Log($"Complete 3"));
            
            // OnNext & OnCompleted & OnError
            subject.Subscribe(
                m => Debug.Log($"4, {m}"),
                error => Debug.Log($"{error}"),
                () => Debug.Log($"Complete 4"));
            
            subject.OnNext(msg);
        }
    }
}
