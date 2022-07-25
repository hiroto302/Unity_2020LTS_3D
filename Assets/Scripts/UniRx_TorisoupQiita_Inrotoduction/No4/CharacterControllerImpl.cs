using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace ToriQiita4
{
    public class CharacterControllerImpl : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        // ジャンプ中フラグ
        private BoolReactiveProperty isJumping = new BoolReactiveProperty();
        
        private void Start()
        {
            // ジャンプ中で無ければ移動可能
            this.UpdateAsObservable()
                .Where(_ => !isJumping.Value)
                .Select(_ => new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")))
                .Where(x => x.magnitude > 0.1f)
                .Subscribe(x => Move(x.normalized));
            
            // ジャンプ中で無いかつ、地面にいればジャンプ可能
            this.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Space) && !isJumping.Value && characterController.isGrounded)
                .Subscribe(_ =>
                {
                    Jump();
                    isJumping.Value = true;
                });
            
            // 着地フラグが変化した時にジャンプ中フラグを戻す
            characterController
                // 着地フラグが変化した時
                .ObserveEveryValueChanged(x => x.isGrounded)
                // 地面の上 かつ ジャンプ中であれば
                .Where(x => x && isJumping.Value)
                // ジャンプフラグを false にする. (ジャンプ状態から着地したことがわかる)
                .Subscribe(_ => isJumping.Value = false)
                .AddTo(gameObject);

            // ジャンプフラグが false になった時 (ジャンプして着地した時)
            isJumping.Where(x => !x)
                .Subscribe(_ => PlaySoundEffect());
        }

        private void Move(Vector3 direction)
        {
            // 移動処理
        }

        private void Jump()
        {
            // ジャンプ処理
        }

        private void PlaySoundEffect()
        {
            // 効果音処理
        }
    }
}
