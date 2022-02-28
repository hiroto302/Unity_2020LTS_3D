using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_WithInterface : MonoBehaviour
{
    [SerializeField] int _maxHP = 0;
    public int HP { get; set; }

    [SerializeField] int _atk = 0;
    public int Atk => _atk;

    void Awake()
    {
        HP = _maxHP;
    }

    // Interface での実装は、継承先で 各クラス毎に 適した処理が実装できる
    public void Attack(ICharacter target)
    {
        target.HP -= Atk * 2;
        if(target.HP <= 0)
            target.Dead();
    }

    public void Dead()
    {
        Debug.Log("Enemy 死亡");
    }
}
