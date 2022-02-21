using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 4. readonly構造体と readonlyメンバー
防御的コピーを防ぐためには、構造体が持つフィールドも readonly など呼び出し元で変更することができないように記述すればよい
C#7.2 から追加された 「readonly構造体」を検討する
readonly構造体にすることで、その構造体が変更不可であることを宣言できる。パフォーマンス的なメリット、そして設計としても、
その構造体は変更不可であることを伝えることができるので優れている。
*/
namespace Structure
{
    public class Struct_Sample2 : MonoBehaviour
    {
        void Start()
        {
            // 3. List型のフィールドを持つ readonly 構造体について
            UpdateMemberStateExample example = new UpdateMemberStateExample(new List<int>());

            // 3.1readonly なので参照先は変えることは出来ない
            // example.NumList = new List<int>();

            // 3.2 Listの中身は更新可能
            example.NumList.Add(1);
        }
    }

    // 1.コンパイルエラーになる readonly 構造体の例
    public readonly struct CompileErrorExample
    {
        // 1.readonly が付与されていないフィールド
        // public int myField;

        // 2.読み取り専用ではないプロパティー
        // public int MyProperty{ get; set;}

        // 3.this の書き換えを行なっている
        public void UpdateSelf()
        {
            // this = new CompileErrorExample();
        }
    }

    // 2.readonly 構造体 の正しい記述の例
    public readonly struct ReadonlyStructExample
    {
        // 1. readonly をフィールドに付与する
        readonly int value;
        // 2. 読み取り専用のプロパティ
        public int Value { get{ return value; } }

        // 3. 自動実装プロパティーはゲーッターオンリー
        public int SecondValue { get; }

        // 4. コンストラクタで、フィールドの初期化
        public ReadonlyStructExample(int initialValue)
        {
            value = initialValue;
            SecondValue = initialValue;
        }

        // ５. コンストラクタ以外で初期化処理を行うようなことは、readonly なので実現不可
        // public void Update(int initialValue)
        // {
        //     value = initialValue;
        //     SecondValue = initialValue;
        // }
    }

    // 3. ただし、readonly 構造体でも List型 の扱いには注意すること
    // 従来通り、readonly でも フィールドやプロパティの状態を更新可能なことに注意する
    public readonly struct UpdateMemberStateExample
    {
        public readonly List<int> NumList { get; }
        public UpdateMemberStateExample(List<int> newlist)
        {
            NumList = newlist;
        }
    }

    /* 4. readonly メンバー、readonly メソッドについて
    「readonly 構造体にしたいが、どうしても一部メンバーだけは更新しないといけない状況」の時
    C#8.0 から加わった 「readonly メンバー」 が有効的である。
    これは、該当メンバーが構造体の状態を更新しないことを宣言する。
    */
    public struct UpdatablePoint
    {
        private float x;
        public float X
        {
            readonly get => x;
            set => x = value;
        }

        private float y;
        public float Y
        {
            readonly get => y;
            set => y = value;
        }

        public UpdatablePoint(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        // readonly 修飾子がついた メソッド
        // 下記の処理で うっかりメンバーを書き換えようとした場合、コンパイルエラーになる。
        // readonly なフィールド や inパラメーターでも、 readonly なメンバーにアクセスする時は防御的コピーは発生しない!
        public readonly float Distance => Mathf.Sqrt( X * X + Y * Y);
        public override readonly string ToString() => $"({X}, {Y})";
    }

}
