using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player は、IBattler と IFieldMover を継承し、実装するので
// 「戦闘」「フィールド移動」することが可能であることが分かる
public class Player_Sample4 : MonoBehaviour, IBattler_Sample4, IFieldMover_Sample4
{
    // ステータス
    [SerializeField] Status_Sample4 _status = new Status_Sample4();
    public Status_Sample4 Status => _status;

    void Awake()
    {
        Status.Initialize();
    }

    void Start()
    {
        // BattleUtility の拡張メソッドを自クラスで使用する時 this をつけて使用
        // this.AttackTarget(targetEnemy);
    }

    // フィールドの移動処理
    void IFieldMover_Sample4.Move(){Debug.Log("フィールド移動");}

    // 明示的 なインターフェースの実装 を行うことで 死亡処理を分けて実装できる
    // フィールド移動中の 死亡処理
    void IFieldMover_Sample4.Dead()
    {
        Debug.Log("フィールド移動中に死亡");
    }

    // 戦闘中の 死亡処理
    void IBattler_Sample4.Dead()
    {
        Debug.Log("戦闘中に死亡");
        Destroy(this.gameObject);
    }
}
