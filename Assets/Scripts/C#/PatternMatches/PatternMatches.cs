using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// パターンマッチ(パターンが示す条件に一致するかを判定)により、制御フローがパワフルになり、かつ簡潔に書けるようになる
// 型やプロパティ、値などによる判定に有効
namespace PatternMathes
{
    // 下記のように、 Shape 型 とそれを継承した Square・Circle・Rectangle 型 があり、これらに手を加えないとする。
    // この状況下で Shape 型を引数にとり、面積を計算する CalculationArea メソッドを新たに定義することを想定する。
    public abstract class Shape {};

    public class Square : Shape
    {
        public float Side { get; set; }
    }
    public class Circle : Shape
    {
        public float Radius { get; set; }
    }
    public class Rectangle : Shape
    {
        public float Width { get; set; }
        public float Height { get; set; }
    }

    public class PatternMatches : MonoBehaviour
    {
        void Start()
        {
            Square square = new Square();
            square.Side = 2.0f;
            Debug.Log(CalculationArea(square) + " : SquareArea");

            int n = 10;
            PatternExample(n);
            string s = "aaa";
            PatternExample(s);
        }

        # region Pattern Matche : 型パターン の使用例
        // パターンマッチを用いらないパターン
        public static float CalculationArea(Shape shape)
        {
            // if文条件式において、is を使用
            // 「変数 is 型名」は、変数がその型であれば、true となる
            if(shape is Square)
            {
                Square square = (Square) shape;
                return square.Side * square.Side;
            }
            else if(shape is Circle)
            {
                Circle circle = (Circle) shape;
                return circle.Radius * circle.Radius * Mathf.PI;
            }
            else if(shape is Rectangle)
            {
                Rectangle rectangle = (Rectangle) shape;
                return rectangle.Width * rectangle.Height;
            }
            else
            {
                throw new System.ArgumentException("サポートされていない shape です");
            }
        }
        // 上記では、if文の条件式で is 演算子を利用して型判定を行った。 判定にマッチした型へキャストし、型毎に面積の計算方法を記述した。
        // 下記には、二度手間に感じた上記の記述をパターンマッチ を利用して記述する。
        // C#7.0 より加わった 「型パターン」を利用。(型名 変数名) is演算子の後に パターンを書けるようになった。
        public static float CalculationArea1(Shape shape)
        {
            // if文条件式において、is を使用
            // 「変数 is 型名 変数名」は、変数がその型であれば、true となる。キャスト処理を省略可能とした。
            if(shape is Square square)
            {
                return square.Side * square.Side;
            }
            else if(shape is Circle circle)
            {
                return circle.Radius * circle.Radius * Mathf.PI;
            }
            else if(shape is Rectangle rectangle)
            {
                return rectangle.Width * rectangle.Height;
            }
            else
            {
                throw new System.ArgumentException("サポートされていない shape です");
            }
        }
        // switch文 と合わせたパターンマッチで実装
        public static float CalculationArea2(Shape shape)
        {
            switch(shape)
            {
                case Square square:
                    return square.Side * square.Side;
                case Circle circle:
                    return circle.Radius * circle.Radius * Mathf.PI;
                case Rectangle rectangle:
                    return rectangle.Width * rectangle.Height;
                default:
                    throw new System.ArgumentException("サポートされていない shape です");
            }
        }
        // パターンマッチを使用した switch 文では、when 句を使用することでより細かい条件判定が可能となる
        public static float CalculationArea3(Shape shape)
        {
            switch(shape)
            {
                case Square square when square.Side == 0f:
                case Circle circle when circle.Radius == 0f:
                case Rectangle rectangle when rectangle.Width == 0f || rectangle.Height == 0f:
                    return 0f;
                default:
                    throw new System.ArgumentException("0以上の値です");
            }
        }
        # endregion

        # region 型パターン、定数パターン、var パターンの使用例
        // object : .NET クラス階層のすべてのクラスをサポートし、派生クラスに下位レベルのサービスを提供する。 これは、全 .NET クラスの基本クラスであり、型階層のルート。
        public static void PatternExample(object argument)
        {
            switch(argument)
            {
                // 定数パターン : 定数との比較判定
                case 0:
                    Debug.Log("定数 0");
                    break;
                // 型パターン と when句の使用
                case int num when num % 2 == 0:
                    Debug.Log("偶数");
                    break;
                // varパターン : 何でもマッチすると判定され、判定した対象を変数で受け取る
                case var X:
                    Debug.Log($"argumentはそれ以外。{X}");
                    break;
            }
        }
        #endregion

        # region タプルパターン、位置指定パターンの使用例

        // ジャンケンを表す列挙型
        public enum Hand
        {
            Rock,
            Paper,
            Scissors
        }
        // ジャンケンの勝利判定メソッド 引数は 2人の Player の手を表すもの
        static string RockPaperScissors(Hand playerFirst, Hand playerSecond)
        {
            // タプルパターン
            switch (playerFirst, playerSecond)
            {
                // タプルの第一要素 が Rock、第二要素が Scissors のときマッチ
                case(Hand.Rock, Hand.Scissors) : return " 1P がグーで勝ち";
                // 他の勝利判定記述省略
                default: return "引き分け";
            }
        }

        // 位置指定パターンでは分解できるオブジェクトに対して利用可能でタプルパターンと似た記述が可能
        public class Hands
        {
            public Hand First { get; set; }
            public Hand Second { get; set; }
            // 分解できるよう Deconstruct メソッドを用意
            // 分解した際の第一要素は First
            // 分解した際の第二要素は Second
            public void Deconstruct (out Hand first, out Hand second) =>
                (first, second ) = (First, Second);
        }
        // 今回の引数では hands クラスを使用
        static string RockPaperScissors1(Hands hands)
        {
            switch(hands)
            {
                // 分解した時の第一要素 hands.First が Hand.Rock かつ 第二要素 hands.Second が Hand.Scissors の時マッチ
                case ( Hand.Rock, Hand.Scissors): return " 1Pがゲームで勝ち";
                default: return " 引き分け";
            }
        }
        # endregion

        # region プロパティーパターン、switch 文の式化、破棄パターン
        // プロパティーパターンでは、対象オブジェクトのプロパティー、フィールドとマッチするかを判定するパターン
        public static float CalculationArea4(Shape shape)
        {
            switch(shape)
            {
                // case Square square when square.Side == 0f:
                // Square 型で かつ Side が 0の時
                case Square { Side: 0f }:
                // case Circle circle when circle.Radius == 0f:
                case Circle { Radius: 0f }:
                // case Rectangle rectangle when rectangle.Width == 0f || rectangle.Height == 0f:
                case Rectangle { Width: 0, Height: 0}:
                    return 0f;
                default:
                    throw new System.ArgumentException("0以上の値です");
            }
        }

        // さらに、C# 8.0 からは switch式が 登場し、switch を式として記述することができるようになった
        // 式であるからには、switch 式は必ず値を返す必要があります。 なので、パターンには網羅性(exhaustiveness)が求められる。
        // すなわち、「どのパターンも満たさずswitch式を抜けてしまう」みたいな状態は許容されない。ちゃんと C# コンパイラーが網羅性をチェックしていて、抜けがあるとコンパイル エラーになります。
        // 多くの場合、末尾にvarパターンか破棄パターンを書いて漏れを防ぐ
        public static float CalculationArea5(Shape shape) => shape switch
        {
            // 型パターン
            Square square => square.Side * square.Side,
            Circle circle => circle.Radius * circle.Radius * Mathf.PI,
            Rectangle rectangle => rectangle.Width * rectangle.Height,

            // 破棄パターン : 何ににもマッチしないと判定され、判定した対象は無視し破棄する
            _ => throw new System.ArgumentException("サポートされていないshape です")
        };

        public static string RockPaperScissors3(Hand first, Hand second) => (first, second) switch
        {
            // タプルパターン
            (Hand.Rock, Hand.Scissors) => "1P がグーで勝ち",
            // 破棄パターン
            _ => "引き分け"
        };

        // その他の記述例
        void DisplayPlusNum(int n)
        {
            var newNum = n switch
            {
                0 => 1,
                1 => 2,
                2 => 3,
                _ => throw new System.InvalidOperationException()
            };
            Debug.Log(newNum);
        }

        // メソッドを実行したい時は、返り値に Action を使用しよう と思ったがやはり だめ
        // void 型を返り値に実行したいがどうもうまくいかん
        // StateSample1 に 実現した方法を記述した
        Action StateSample0(Hand hand) => hand switch
        {
            // Hand.Rock => Debug.Log("ぐー"),
            _ => throw new System.InvalidOperationException()
        };

        // string を介して他のメソッドを実行する処理を記述
        void StateSample1(Hand hand)
        {
            var callBackMethod = hand switch
            {
                Hand.Paper => nameof(CallBackMethodSample),   // 文字列型の返り値が action に代入される
                _ => throw new System.InvalidOperationException()
            };
            // 文字列を介して処理を実行
            SendMessage(callBackMethod);
        }

        void CallBackMethodSample()
        {
            Debug.Log("呼ばれたよ");
        }

        # endregion
    }
}
