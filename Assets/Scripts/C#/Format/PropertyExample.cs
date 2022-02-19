using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Format
{
    public class PropertyExample : MonoBehaviour
    {
        // プロパティーのバッキングフィールド
        float myBackingField;
        // 読み取り専用プロパティー
        public float ReadOnlyProperty => myBackingField;
        // 冗長なので上記の方が好ましい

        public float AnotherReadOnlyProperty
        {
            get => myBackingField;
        }

        // 書き込み専用プロパティー
        public float WriteOnlyProperty
        {
            set => myBackingField = value;
        }

        // 読み取り書き込みプロパティー
        public float ReadWriteProperty
        {
            get => myBackingField;
            set => myBackingField = value;
        }
        // 従来の記述 (上記と大差ない。読み取り専用の時はいいかも。以外に上記の方が記述は短いから使っていこう)
        public float MyBackingField
        {
            get {  return myBackingField; }
            set { myBackingField = value; }
        }

        void Start()
        {
            Rect1 rect1  = new Rect1(1.0f, 1.0f);
        }
    }

    /* 自動実装プロパティーの強化
    自動実装プロパティー : コード上にフィールドないが、内部的にコンパイラーがフィールドを生成している。
    「自動実装プロパティのバッキングフィールド」とは、コードからは見えない、コンパイラーが生成した、プロパティを実現するうえで必要なフィールドである

    上記のものが以下の３点できるようになった。
    1. 読み取り専用自動実装プロパティーが、コンストラクタで初期化可能になった
    2. 読み取り書き込み自動実装プロパティーがで、フィールド初期化子を使えるようになった
    3. 自動実装プロパティーのバッキングフィールドに属性をつけれるようになった
    */
    public class Rect
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public Rect(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
    // 1 の内容について
    // 上記のクラスでは、Width・Height プロパティーが書き換えることができる。これをコンストラクターで初期化した値を書き換えできないよう 読み取り専用にする。
    public class Rect1
    {
        private readonly float width;
        public float Width { get{ return width; }}
        private readonly float height;
        public float Height { get{ return height; } }

        public Rect1(float width, float height)
        {
            this.width = width;
            this.height = height;
        }
    }
    // 従来の方法では上記のようにしか出来なかったが、C#6.0 以降では、下記のように
    // getアクセサリーのみを持つ自動実装プロパティーでも、コンストラクでのみ初期化が可能になった
    public class Rect2
    {
        public float Width { get; }

        public float Height { get; }

        // コンストラクタで初期化
        public Rect2(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }
        // メソッドでは初期化不可能
        void Initialize(float width, float height)
        {
            // Property or indexer 'Rect2.Width' cannot be assigned to -- it is read only のエラーが出る
            // this.Width = width;
            // this.Height = height;
        }
    }
}
