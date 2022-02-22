using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UniRx.Triggers;


/* InputEventProvider の実装
このクラスは、UnityEngin.Input の 「入力イベント」 を
IInputEventProvider で定義された「操作イベント」に変換する。

操作する Player キャラクタに アタッチされているコンポーネントにおいて、
UnityEngine.Input に依存する箇所はこのクラスのみに制限する。
*/

namespace UniRxSampleGame.Players.InputImpls
{
    public class InputEventProviderImpl : MonoBehaviour, IInputEventProvider
    {
        // 長押しだと判定するまでの時間
        private static readonly float LongPressSeconds = 0.25f;

        #region  IInputEventProvider

        // プロパティ はメソッドだから => だよ
        public IObservable<Unit> OnLightAttack => _lightAttackSubject;
        public IObservable<Unit> OnStrongAttack => _strongAttackSubject;
        public IReadOnlyReactiveProperty<bool> IsJump => _jump;
        public IReadOnlyReactiveProperty<Vector3> MoveDirection => _move;

        #endregion

        // イベント発行に利用するSubject や ReactiveProperty
        readonly Subject<Unit> _lightAttackSubject = new Subject<Unit>();
        readonly Subject<Unit> _strongAttackSubject = new Subject<Unit>();
        readonly ReactiveProperty<bool> _jump = new ReactiveProperty<bool>(false);
        readonly ReactiveProperty<Vector3> _move = new ReactiveProperty<Vector3>();

        void Start()
        {
            // OnDestroy時に Dispose() されるように登録
            _lightAttackSubject.AddTo(this);
            _strongAttackSubject.AddTo(this);
            _jump.AddTo(this);
            _move.AddTo(this);

            // 攻撃処理 : 攻撃ボタンの長押し具合で 弱/強 攻撃を分岐
            // Update() を Observable に変換
            this.UpdateAsObservable()
                // Attackボタンの状態を取得
                .Select(_ => Input.GetButton("Attack"))
                // 値が変動した場合のみ通過
                .DistinctUntilChanged()
                // 最後に状態が変動してからの経過時間を付与
                .TimeInterval()
                // Subscribe直後の値は無視
                .Skip(1)
                .Subscribe(t =>
                {
                    // 攻撃ボタンを押した瞬間のイベントは無視
                    if(t.Value) return;

                    // 攻撃ボタンを押してから離すまでの時間を判定
                    if (t.Interval.TotalSeconds >= LongPressSeconds )
                    {
                        _strongAttackSubject.OnNext(Unit.Default);
                    }
                    else
                    {
                        _lightAttackSubject.OnNext(Unit.Default);
                    }
                }).AddTo(this);
        }
        void Update()
        {
            // ジャンプボタンの押し具合を反映
            _jump.Value = Input.GetButton("Jump");

            // 移動入力をベクトルに変換して反映
            // ReactiveProperty.SetValueAndForcenotify : 強制的にメッセージを発行
            _move.SetValueAndForceNotify(new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical")));
        }
    }
}
