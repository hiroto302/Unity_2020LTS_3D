using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

// player の移動処理
// InputEventProviderImpl を利用しているコンポーネントの一つが PlayerMove.
// InputEventProviderImpl から発行された「操作イベント」を利用して、移動処理を実装する
namespace UniRxSampleGame.Players
{
    public class PlayerMove : MonoBehaviour
    {
        // 接地状態
        public IReadOnlyReactiveProperty<bool> IsGrounded => _isGrounded;
        // 落下中であるか
        public bool IsFall => _rigidbody.velocity.y < 0;
        // 移動速度
        [SerializeField] float _dashSpeed = 3;
        // ジャンプ速度
        [SerializeField] float _jumpSpeed = 5.5f;
        // Raycast時のPlayer の高さを補正する
        [SerializeField] float _characterHeightOffset = 0.15f;
        // 接地判定に利用するレイヤ設定
        [SerializeField] LayerMask _groundMask;

        readonly RaycastHit[] _raycastHitResult = new RaycastHit[1];
        readonly ReactiveProperty<bool> _isGrounded = new BoolReactiveProperty();
        // PlayerCore _playerCore;
        Rigidbody _rigidbody;
        bool _isJumpReserved;
        IInputEventProvider _inputEventProvider;
        bool _isMoveBlock;

        void Start()
        {
            // UniRx を扱うときは、下記のような処理を忘れないように
            _isGrounded.AddTo(this);

            _rigidbody = GetComponent<Rigidbody>();
            // _playerCore = GetComponent<PlayerCore>();
            // IInputEventProvier を継承している InputEventProvierImpl コンポーネントが Playerアタッチされている
            _inputEventProvider = GetComponent<IInputEventProvider>();
        }

        // rigidbody に対する操作は FixedUpdate を用いる
        // 操作イベントの更新のタイミングは Update であるが、
        // 現在値を参照するなら問題はない
        void FixedUpdate()
        {
            // 接地判定処理
            CheckGrounded();

            // 上書きする移動速度の値
            var vel = Vector3.zero;

            // Player が死んでないなら操作を反映する
            // if (!_playerCore.IsDead.Value)
            // {
                    // 操作イベントから得られた移動量
                    var moveVector = GetMoveVector();
                    // 移動操作を反映する
                    // if (moveVector != Vector3.zero && _isGrounded.Value)
                    if (moveVector != Vector3.zero)
                    {
                        vel = moveVector * _dashSpeed;
                    }

                    // ジャンプ
                    // if (_inputEventProvider.IsJump.Value && _isGrounded.Value)
                    if (_inputEventProvider.IsJump.Value)
                    {
                        vel += Vector3.up * _jumpSpeed;
                        _isJumpReserved = false;
                    }
                    // 重力落下分を維持する
                    vel += new Vector3(0, _rigidbody.velocity.y, 0);
                    // 速度を更新
                    _rigidbody.velocity = vel;

                    // 落下中は無敵判定
                    // _playerCore.SetInvincible(IsFall);
            // }
        }

        // 操作イベントの値から移動量を決定する
        Vector3 GetMoveVector()
        {
            // InputEventProviderImpl.cs のUpdate()で実装されている移動量を取得している
            var x = _inputEventProvider.MoveDirection.Value.x;
            if(x > 0.1f)
            {
                return Vector3.right;
            }
            else if (x < -0.1f)
            {
                return -Vector3.right;
            }
            else
            {
                return Vector3.zero;
            }
        }
        // 接地判定処理
        void CheckGrounded()
        {
            // 地面に対して Raycast を飛ばして接地判定する
            // var hitCount = 
        }

        // 移動不可フラグ
        public void BlockMove(bool isBlock)
        {
            _isMoveBlock = isBlock;
        }

    }
}
