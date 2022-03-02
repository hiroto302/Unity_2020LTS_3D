using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace UniRxInputSystemSample
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody _rb;
        // IInputEventProvider を介して入力されている値を取得
        IInputEventProvider _inputEventProvider;

        // 移動速度
        [SerializeField] float _moveSpeed = 3.0f;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _inputEventProvider = GetComponent<IInputEventProvider>();
        }

        void Update()
        {
            Debug.Log(_inputEventProvider.DisplayLog.Value);
            DisplayLog();
        }
        void FixedUpdate()
        {
            MovePlayer();
        }

        void MovePlayer()
        {
            // いつものように Input.System をベタ書きしない
            // IInputEventProvider を介して移動 方向を取得
            _rb.velocity = new Vector3(
                    _inputEventProvider.MoveDirection.Value.x * _moveSpeed,
                    _rb.velocity.y,
                    _inputEventProvider.MoveDirection.Value.z * _moveSpeed);
        }

        void DisplayLog()
        {
            // IInputEventProvider を介して A Key が入力された時に実行されるメソッドを書く
            if(_inputEventProvider.DisplayLog.Value)
            {
                Debug.Log("Aが押されたよ");
            }
        }
    }
}

