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

            Player1 player1 = new Player1();
            Debug.Log(player1.Name);
            Debug.Log(player1.Level);
            Player2 player2 = new Player2();
            Debug.Log(player2.Name);
            Debug.Log(player2.Level);

            Debug.Log(Example2.MyProperty1 + " : ex2.property1");
            Example2.DisplayNum();
        }
        [SerializeField] BackingFieldAttributeExample Example = new BackingFieldAttributeExample();
        [SerializeField] BackingFieldAttributeExample1 Example1 = new BackingFieldAttributeExample1();
        [SerializeField] BackingFieldAttributeExample2 Example2 = new BackingFieldAttributeExample2();

    }

    /* 自動実装プロパティーの強化
    自動実装プロパティー : コード上にフィールドないが、内部的にコンパイラーがフィールドを生成している。
    「自動実装プロパティのバッキングフィールド」とは、コードからは見えない、コンパイラーが生成した、プロパティを実現するうえで必要なフィールドである

    上記のものが以下の３点できるようになった。
    1. 読み取り専用自動実装プロパティーが、コンストラクタで初期化可能になった
    2. 読み取り書き込み自動実装プロパティーが、フィールド初期化子を使えるようになった
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

    // 2. 読み取り書き込み自動実装プロパティーが、フィールド初期化子を使えるようになった について
    public class Player
    {
        public string Name { get; set; }
        public int Level { get; set; }
    }
    // 初期値を与えたい時、C#6.0 より以前では 自動実装プロパティーを諦めなくてはならなかった。
    public class Player1
    {
        // 以下のような記述では、初期化されていない。他クラスで Name を参照すると Null となる
        private string name = "No Name";
        public string Name{ get; set; }
        // こちらは、初期化されてるものを返す。
        private int level = 1;
        public int Level
        {
            get { return level; }
            set { level = value ; }
        }
    }
    // C#6.0 以降では フィールド初期値を使用することで設定できるようになった!
    public class Player2
    {
        public string Name { get; set; } = "Player2 Name";
        public int Level { get; set; } = 2;
    }

    // 3. 自動実装プロパティーのバッキングフィールドに属性をつけれるようになった ことについて
    [System.Serializable]
    public class BackingFieldAttributeExample
    {
        // 下記の変数については、シリアライズ化されない
        [System.NonSerialized] public int myProperty;
        [SerializeField] int myProperty1;
        public int MyProperty { get; private set; }
    }
    // C#7.3 より前では, 自動実装プロパティーの バッキングフィールドに NonSerialized などの属性を付与することがで出来なっかため下記のように記述するしかなかった
    [System.Serializable]
    public class BackingFieldAttributeExample1
    {
        [System.NonSerialized]
        int field;
        public int MyProperty
        {
            get { return field; }
            set { field = value; }
        }
        [SerializeField]
        int field1;
        public int MyProperty2
        {
            get { return field1; }
            set { field1 = value; }
        }
    }
    // 上記の記述が下記のようにできるようになった
    [System.Serializable]
    public class BackingFieldAttributeExample2
    {
        [field: System.NonSerialized]
        public int MyProperty { get; private set; }
        [field: SerializeField]
        public int MyProperty1 { get; private set; }
        public void DisplayNum()
        {
            Debug.Log(MyProperty1);
        }
    }
    /*

    ここまで記述してきたが、[field: System.NonSerialized]は、そもそも Property に関しては Inspecter 上に表示されない。どこで有用的に使うことができるのか分からない。
    変更しないようにすること明示的するためか？
    しかし、[field: SerializeField] は プロパティーを表示することができるから有効できるから有効的だ！と思えるがこの使用方法は問題があり避けるべきである
    BackingFieldAttributeExample2 の インスタンス を生成し、Inspector 上の値を確認すると、 My Property1 : 0 となっている。 My Property1 というような名称でシリアライズ化されている。
    <MyProperty1>k__BackingField : 0 は、自動実装プロパティーの内部的に生成されるバッキングフィールドの名称である。コードからアクセスできず、
    普通のフィールドとしては使えない名称のフィールドとなっている
    これに、SerializeField を付与すると、普通の名称としては使えない名称でシリアライズされることに注意しなければならない。
    今使っている限り、インスペクタ上の表示される名称は改良されている。<MyProperty1>k__BackingField のような名称で表示されていた。
    データ(YAML形式)の方を確認すると次のような名前でシリアライズ化されている

    Example2:
    <MyProperty1>k__BackingField: 20

    「シリアライズされる名前」・「シリアライズされたり、永続化される際のデータフォーマット」という観点からは、やはり考慮しないといけないかも
    現状の [field: SerializeField] を使うのは、特にプロダクトでは避けたほうが賢明。
    バッキングフィールドの内部表現が<***>k__BackingFieldであり続ける保証がないので、シリアライズしたデータと齟齬が生まれる可能性を排除できないから。

    なので、

    [SerializeField] private int level;
    public int Level => level;

    のような書き方をしていこう!
    「SerializeFieldをつけた時のシリアライズされる名前はフィールド名で」っていう原則が崩さないようにする

    */
}
