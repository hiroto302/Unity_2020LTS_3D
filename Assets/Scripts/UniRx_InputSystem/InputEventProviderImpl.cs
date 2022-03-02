using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

/* InputEventProvider の実装
このクラスは、UnityEngin.Input の 「入力イベント」 を
IInputEventProvider で定義された「操作イベント」に変換。

操作する Player キャラクタに アタッチされているコンポーネントにおいて、
UnityEngine.Input に依存する箇所はこのクラスのみに制限する。
*/
namespace UniRxInputSystemSample
{
    public class InputEventProviderImpl : MonoBehaviour, IInputEventProvider
    {

        // IInputEventProvier : 操作イベント
        public IReadOnlyReactiveProperty<Vector3> MoveDirection => _move;
        public IReadOnlyReactiveProperty<bool> DisplayLog => _displayLog;

        #region  操作イベントに利用する変数
        readonly ReactiveProperty<Vector3> _move = new ReactiveProperty<Vector3>();

        // A Key が押されたか
        readonly ReactiveProperty<bool> _displayLog = new ReactiveProperty<bool>();

        #endregion

        void Start()
        {
            // Dispose されるようにすることを登録し忘れないように
            _move.AddTo(this);
            _displayLog.AddTo(this);
        }

        void Update()
        {
            _move.Value = InputAxisValueToMoveDirection();
            _displayLog.Value = InputKeyDownAToDiplayLog();
        }

        // Input.GetAxis 入力により 決定する移動方向
        Vector3 InputAxisValueToMoveDirection()
        {
            return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }

        // A Key が Down (押されたか) したか
        bool InputKeyDownAToDiplayLog()
        {
            return Input.GetKeyDown(KeyCode.A);
        }
    }

}
