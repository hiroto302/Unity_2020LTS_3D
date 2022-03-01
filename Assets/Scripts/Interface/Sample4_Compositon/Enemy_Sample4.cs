using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy は IBattler を継承し、実装しているので 「戦闘することができる」 ことがわかる
public class Enemy_Sample4 : MonoBehaviour, IBattler_Sample4
{
    // ステータス
    [SerializeField] Status_Sample4 _status = new Status_Sample4();
    public Status_Sample4 Status => _status;

    void Awake()
    {
        Status.Initialize();
    }

    // 死亡処理
    public void Dead()
    {
        Destroy(this.gameObject);
    }

}
