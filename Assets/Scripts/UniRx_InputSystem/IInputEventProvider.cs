using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace UniRxInputSystemSample
{
    // 操作イベントを発行するインターフェース
    public interface IInputEventProvider
    {
        // 移動操作 : Player の移動方向
        IReadOnlyReactiveProperty<Vector3> MoveDirection { get; }
        // ログを表示するか ( A Key を押した時 )
        IReadOnlyReactiveProperty<bool> DisplayLog { get; }
    }
}
