using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1_WithInterface : MonoBehaviour, ICharacter
{
    [SerializeField] int _maxHP = 0;
    public int HP { get; set; }

    [SerializeField] int _atk = 0;
    public int Atk => _atk;

    void Awake()
    {
        HP = _maxHP;
    }

    // Interface での実装は、継承先で 各クラス毎に 適した処理ができる
    public void Attack(ICharacter target)
    {
        target.HP -= Atk;
        if(target.HP <= 0)
            target.Dead();
    }

    public void Dead()
    {
        Debug.Log("Player 死亡");
    }
}
