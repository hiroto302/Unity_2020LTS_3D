using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Null 演算子について
namespace Null
{
    [System.Serializable]
    public class Status
    {
        public int HP;
        public Status(int hp)
        {
            HP = hp;
        }
    }

    public class Monster : MonoBehaviour
    {
        public string Name { get; set;}
        [SerializeField] Status status = new Status(10);
        Monster monster = null;
        public static event Action action;
        List<Monster> monsterList;
        [SerializeField] GameObject sampleMonster;

        void Start()
        {
            // このまま下記の処理を行うと monster = null なので NullReferenceException Error になる
            // monster.Name = " sample1 ";
            // Debug.Log(monster.Name);

            // 条件式 2項演算子による Null チェック
            string monsterName = monster == null ? null : monster.Name;
            Debug.Log(monsterName + " : monater name 0");

            // C# 6.0 から追加された null 条件演算子「?.」 で記述
            string monsterName1 = monster?.Name;
            Debug.Log(monsterName1 + " : monster name 1");

            // 条件演算子 「?.」の連結利用
            int? monsterNameLength = monster?.Name?.Length;
            Debug.Log(monsterNameLength + " : nameLength");

            // Action・UnityAction などのデリーゲート呼び出しにも null 演算子を利用、 action == null の時は何も実行されない
            action?.Invoke();

            // インデクサーに対しての利用
            Monster firstMonser = monsterList?[0];

            // もしも, null の時デフォルトの値を設定する記述を C#2.0 からあった null 合体演算子「??」を利用して記述
            // 従来の記述 ２項演算子
            monsterName = monster == null ? "NoName" : monster.Name;
            // 合体演算子 「??」
            monsterName1 = monster?.Name ?? "NoName";

            // C#8.0 から加わった、 null 合体割り当て演算子 「??=」を利用
            // 左オペランドが null ならば 右オペランドの値を代入
            // もし左オペランドが 非null ならば、右オペランドは評価されない
            monsterName1 ??= "NoNmae";

            /*
            ここまで紹介した 「?.」はComponent クラスを継承したクラスや GameObject クラスでの利用を注意すること
            上記の２つのクラスは、「==」や「!=」がオーバーロードされ独自実装されている
            そのため, 下記の２つの記述が等価ではなくなってしまっている

            null ではなかった monster を破壊した時
            Destroy(monster);
            // 二項演算
            Debug.Log(monster == null ? null : monster.name); Null が表示される
            Debug.Log(monster?.name); MissingReferenceException が投げられる
            */

            // DestroyImmediate(sampleMonster);
            // Debug.Log(sampleMonster?.name);
        }
    }
}


