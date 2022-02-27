using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 記事投稿者用のQiitaPostインターフェイス
interface IAuthorQiitaPost
{
    // int LGTMCount { get; }
    // int StockCount { get; }

    // 投稿者が行う 記事の削除
    void Delete();
}
