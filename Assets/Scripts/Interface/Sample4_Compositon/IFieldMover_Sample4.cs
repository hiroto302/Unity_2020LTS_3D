using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フィールドを「移動することが可能(できる)」ことを表している Can - do 関係
interface IFieldMover_Sample4
{
    // フィールドの移動処理
    void Move();
    // フィールド移動中に 死亡した時の処理
    void Dead();
}
