using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReferenceType
{
    public class ReferenceType_Sample : MonoBehaviour
    {
        void Start()
        {

        }
    }

    // readonly 修飾子について
    public class ReadOnlyExample
    {
        public readonly string readonlyField;
        readonly List<string> readonlyFieldList;

        public ReadOnlyExample(string initialValue)
        {
            readonlyField = initialValue;
            readonlyFieldList.Add(initialValue);
        }

        public void Update(string updateValue)
        {
            // readonly フィールドは上書きできない
            // readonlyField = updateValue;

            // 上記のように上書きは出来ない
            // readonlyField[0] = updateValue;
            // ただし、状態の更新は可能
            readonlyFieldList.Add(updateValue);
        }
    }
}
