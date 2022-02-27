using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IQiitaPost
{
    /// <summary>
    /// LGTM数を取得する
    /// </summary>
    int LGTMCount { get; }
    /// <summary>
    /// ストック数を取得する
    /// </summary>
    int StockCount{ get; }

}
