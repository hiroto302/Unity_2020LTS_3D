using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BattleUtility の使用例
public class BattleManger_Sample4 : MonoBehaviour
{
    [SerializeField] Player_Sample4 _player = null;
    [SerializeField] Enemy_Sample4 _enemy = null;

    void Start()
    {
        // クラスを渡しても インターフェースにキャストしてくれる
        BattleUtility_Sample4.Attack(_player, _enemy);

        // 拡張メソッドの利用
        // 第一引数に Player_Sample4 が代入されているイメージ
        _player.AttackTarget(_enemy);
        _enemy.AttackTarget(_player);
    }
}
