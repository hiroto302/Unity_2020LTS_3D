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


    }
}
