using UnityEngine;
/* Utility クラス
共通処理をまとめる

Sample1 の Player と Enemy に実装されていた Attack メソッド を Utility クラスにまとめた
Attack が Player と Enemy では処理内容が異なる時は、下記のような記述はしない。
ダメージ計算だけ共通化するなど、様々なことを模索して実装すること
*/
public static class BattleUtility_Sample4
{
    public static void Attack(IBattler_Sample4 attacker, IBattler_Sample4 target)
    {
        target.Status.HP -= attacker.Status.Atk;
        if(target.Status.IsDead())
        {
            target.Dead();
        }
    }

    // 引数に this を使用することで拡張メソッドとなる
    public static void AttackTarget(this IBattler_Sample4 attacker, IBattler_Sample4 target)
    {
        target.Status.HP -= attacker.Status.Atk;
        if(target.Status.IsDead())
        {
            target.Dead();
        }
    }
}
