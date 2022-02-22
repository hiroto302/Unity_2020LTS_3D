using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;


/* 操作イベント
ここでいう操作イベントとは、遊ぶ人が Player キャラクターの制御に用いる入力と意味する
キーボードの入力などから「発行された入力イベント」をもとに、
Player キャラクター を実際にどう動かすかという 「操作イベント」に変換すること目指して、playerを制御する機能実装する
*/

/* インターフェース定義
操作イベントの抽象化レイヤとして、下記のインターフェースを定義する。

利点の一つは、入力システムの差し替えが後から行いやすい

入力イベントを受け取る方法として、 「UnityEngin.Input」 があるが、これを様々な箇所でベタ描きすると、
後から、入力システムを差し替えたくなった時、変更する箇所が増えてしまう。
またどの「入力イベント」にどこが対応しているか把握も難しくなってしまう。

これらを防ぐために、入力イベントの変換レイヤを挟み、「操作イベント」という形で提供するようにする

抽象化レイヤー : 高レベルのリクエストを、そのリクエストを実行するために必要な低レベルのコマンドに変換するメカニズ。
また、ある特定の機能の詳細な実装を隠す手段
*/

namespace UniRxSampleGame.Players
{
    // 操作イベントを発行するインターフェース
    // 操作操作イベントを Observable, IReadonlyReactiveProperty として提供する
    public interface IInputEventProvider
    {
        // 移動操作
        IReadOnlyReactiveProperty<Vector3> MoveDirection { get; }
        // 弱攻撃
        IObservable<Unit> OnLightAttack { get; }
        // 強攻撃
        IObservable<Unit> OnStrongAttack { get; }
        // ジャンプ
        IReadOnlyReactiveProperty<bool> IsJump { get; }
    }
}

