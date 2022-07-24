using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ToriQiita2
{
    /// <summary>
    /// OnErrorをSubscribeが検知するとストリームの購読を中止してしまう ことを表すクラス
    /// </summary>
    public class OnErrorSample : MonoBehaviour
    {
        private void Start()
        {
            ExecuteSample1();
            ExecuteSample2();
        }

        private void ExecuteSample1()
        {
            var stringSubject = new Subject<string>();
            
            // 文字列をストリームの途中で整数に変換する
            stringSubject
                // 数値を表現する文字列以外の場合は例外が発生する
                .Select(str => int.Parse(str))  
                .Subscribe(
                    X => Debug.Log($"整数に変換成功 {X}"),
                    ex => Debug.Log($"整数に変換失敗 {ex}"),
                    () => Debug.Log($"Complete"));
            
            stringSubject.OnNext("1");
            stringSubject.OnNext("例外");
            // 例外が発生するとストリームの購読を中止する。なのでこれ以降の処理は Subject を呼び出しても実行されない
            stringSubject.OnNext("2");
            stringSubject.OnCompleted();
        }
        
        private void ExecuteSample2()
        {
            var stringSubject = new Subject<string>();
            
            stringSubject
                .Select(str => int.Parse(str))
                // 例外の型指定でフィルタリングが可能
                .OnErrorRetry((FormatException ex) => Debug.Log($"例外が発生したため再購読します"))
                .Subscribe(
                    X => Debug.Log($"整数に変換成功 {X}"),
                    ex => Debug.Log($"整数に変換失敗 {ex}"),
                    () => Debug.Log($"Complete"));
            
            stringSubject.OnNext("1");
            stringSubject.OnNext("例外");
            // ストリームを再構築して購読を続行させている。なので、これ以降の処理も実行される
            stringSubject.OnNext("2");
            stringSubject.OnCompleted();
        }
    }
}