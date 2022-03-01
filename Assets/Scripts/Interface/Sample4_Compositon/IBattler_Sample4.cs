/* Can - do 関係 で表現したインターフェース

IBattler を実装するクラスは、「戦闘ができる」ことを表している

*/
public interface IBattler_Sample4
{
    // コンポジション Status
    Status_Sample4 Status { get; }
    // 戦闘中に 死亡した時の処理
    void Dead();
}
