using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
