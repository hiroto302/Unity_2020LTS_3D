using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// このインターフェースを 参照する場合は、public にすること
public interface ICharacter
{
    int HP { get; set; }
    int Atk { get; }
    // 引数に ICharacter 型の変数をとる 「Attack」というメソッドを定義せよ
    // public interface インターフェース名 のようにすること
    // public でなければ、このメソッドを実装する側で CS0051 エラー が生成される
    void Attack(ICharacter target);
    void Dead();
}
