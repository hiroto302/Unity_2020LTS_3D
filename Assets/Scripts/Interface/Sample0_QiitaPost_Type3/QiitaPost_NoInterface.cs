using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* ３つのタイプのインターフェイス
インターフェイスは、以下の３つのタイプに分けらる

1. 疎結合を目的としたインターフェイス
2. クラスの機能を保証することを目的としたインターフェイス
3. クラスへの安全なアクセスを提供することを目的としたインターフェイス

「インターフェイスは、実装するクラスにメソッドの実装を強制するもの」とういう説明では不十分である
(複数人で作業している時に、そのメソッドを実装するために扱う変数が一目で分かるなど利点がある)

では何なのかというと、「クラスにアクセスするためのインターフェイス」と言える。

では、なぜ必要なのか考えてみる。
「クラスにアクセスするためのインターフェイス？別にインターフェイスがなくても普通にインスタンス名.メンバ名でアクセスできるが？」
ところが、インターフェイスがないと大きな問題となる場合がたくさんある。

下記に、インターフェースを使用しない クラス QiitaPost を実装してみる

実装したクラスを、記事を書く人(QiitaAuthor)がインスタンスを生成し、サーバーにアップロードする。
そして、皆んな(QittaReader)に読んでもらう事が可能となる

ここで問題なのは、書く人自身が自分の記事に LGTM できること。また、読み手側が 勝手に記事を削除できてしまうことである。
この問題が発生してしまうのは,

「QiitaPostクラスにアクセスするための適切なインターフェイスが定義されていないから」である。

インターフェイスがなく、QiitaPostクラスの インスタンス名.メンバ名でアクセスができしまっている状態である。

自販機で言えば、「売上金をすべて排出する」ボタンが客が触れられる場所に配置してあるようなもの。
普通の自販機は客用のインターフェイスと管理者用のインターフェイスが完全に分けられ、客は「売上金をすべて排出する」ボタンを押すことはできない。
それと同じで、「クラスにも使用者に応じて適切なインターフェイス」が定義されていないと、あとあと問題が発生することがある。
これが、C#の「インターフェイス」の本質的な意味である。

上記の問題が起こらないように、 QittaPostクラス のインターフェースを定義し、実装する。

記事を書くために必要な機能を IAuthorQiitaPost
記事を閲覧する人のために必要な機能を IReaderQiitaPost
被っているメンバは更に抽象的な IQiitaPostインターフェイス にまとめた

に記述する。
*/

public class QiitaPost_NoInterface
{
    string _title;
    string _text;

    // タイトル と 本文 を入力して記事を作成
    public QiitaPost_NoInterface(string title, string text)
    {
        this._title = title;
        this._text = text;
    }

    public Uri PostURL { get; }                     // 記事の URL
    public string Title => _title;                  // タイトル
    public string Text => _text;                    // 本文
    public int LGTMCount { get; private set; }      // LGTM数
    public int StockCount { get; private set; }     // ストック数

    // LGTM するメソッド
    public void LGTM()
    {
        ++ LGTMCount;
    }
    public void Stock()
    {
        ++ StockCount;
    }

    // 記事を削除するメソッド
    public void Delete()
    {
        _title = string.Empty;
        _text = string.Empty;
    }

}
