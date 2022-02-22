using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;

/* uGUIイベントからの変換
uGIコンポーネントが発行するイベントを OnNextメッセージに変換することができる

下記に、uGUIコンポーネントである Button のクッリクイベントを Observable として扱う
*/
namespace UniRxSample
{
    public class UnityEventToObservable : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private IDisposable _disposable;

        private void Start()
        {
            _disposable = _button
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    Debug.Log   ("ボタンがクリックされたよ");
                });
        }

        void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}
