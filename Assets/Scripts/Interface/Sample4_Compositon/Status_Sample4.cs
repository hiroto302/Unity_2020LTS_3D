using UnityEngine;

/* Has - a 関係で表現したクラス (コンポジション)

このクラスのインスタンスを持つクラスは、 「Status」を持つ ことを表している

*/

[System.Serializable]
public class Status_Sample4
{
    [SerializeField] int _maxHP = 0;
    public int HP {get; set;}

    [SerializeField] int _atk = 0;
    public int Atk => _atk;

    public void Initialize()
    {
        HP = _maxHP;
    }

    // 死亡したか
    public bool IsDead()
    {
        return HP <= 0;
    }


}
