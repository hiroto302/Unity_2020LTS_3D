using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 式形式
namespace Format
{
    public class Sphere
    {
        public float Radius { get; private set; }
        public Vector3 Center { get; private set; }

        public Sphere(float radius, Vector3 center)
        {
            Radius = radius;
            Center = center;
        }

        /*
        本質的に一行で記述できるものを、式形式のメンバー定義を利用し簡潔に記述する
        式形式メンバー定義を使うと、助長なブランケット {} や return などを省略し本質的な記述のみで実装が行える
        これが使えるのは、次のメンバーである
        メソッド・読み取り専用プロパティー(C#6.0から)・プロパティー(C#7.0から)・コンストラクター・ファイなライザー・インデクサー
        */

        // public float Volume
        // {
        //     get
        //     {
        //         return 4.0f / 3.0f * Mathf.PI * Radius * Radius;
        //     }
        // }
        public float Volume => 4.0f / 3.0f * Mathf.PI * Radius * Radius;

        // public void Move(Vector3 center)
        // {
        //     Center = center;
        // }
        public void Move(Vector3 center) => this.Center = center;
    }
}
