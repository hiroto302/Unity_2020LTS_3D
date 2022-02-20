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

    }
}
