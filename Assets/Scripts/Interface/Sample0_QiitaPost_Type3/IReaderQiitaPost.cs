using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 記事閲覧者用のQiitaPostインターフェイス
interface IReaderQiitaPost
{
    // 取得する 記事の タイトル・本文・LGTM数・ストック数
    string Title { get; }
    string Text { get; }

    // int LGTMCount { get; }
    // int StockCount { get; }

    // 閲覧者が行う LGTM と Stock
    void LGTM();
    void Stock();
}
