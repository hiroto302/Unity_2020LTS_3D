using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* インターフェースを用いて 実装した QittaPost クラス

インターフェイスに定義されたすべてのメンバが、QiitaPostクラスに実装されているのでエラーが出ない。
一つでも実装されていないメンバがあるとコンパイルエラー となる。
これが冒頭で説明した「インターフェイスは、実装するクラスにメソッドの実装を強制するもの」という説明に通じる。

インターフェイスを作成したので、「Qiitaの記事を作成する人」と「Qiitaの記事を読む人」には
QiitaPostクラスに直接アクセスするのをやめてもらい、きちんとインターフェイス経由でアクセスしてもらう
このインターフェースを 利用して

記事を投稿する人 QittaAutor
記事を読む人 QittaReader     の実装を確認してみる。

問題であった、書く人自身が自分の記事に LGTM できること。また、読み手側が 勝手に記事を削除できてしまうことが
インターフェースを介することで防ぐことが出来た。

クラスを作るときには「誰に、どのように使ってほしいか」を意識した上で、適切にインターフェイスを用意することがとても重要である事がわかる。
適切なインターフェイスを用意し、使う側がきちんと然るべきインターフェイス経由でクラスにアクセスするようにすることで、
想定外の使い方をされて不具合が発生することを防ぐことができる。

「3.クラスへの安全なアクセスを提供することを目的としたインターフェイス」 のタイプを作成することが出来た。
*/

/* クラスを作る前にインターフェイスから作る
クラスを作成する前にまずインターフェイスから設計することが好ましいかもしれない。

クラスの使い手と使い方を意識して、まずクラスのインターフェイスを作成。
それが終わったあとに、クラスに実装させて、エラーが出なくなるまで内部の実装を書くという流れを心がけると、
うっかりクラスに直接アクセスされてしまった！なんてことが少なくなるはず。

また、インターフェイスの設計は、クラス内部でどうやって実装しようかなどと考えることもなく、必要な機能をただ列挙していくだけなので
必要な機能を抜かりなく記述することができるというメリットもあります。

たとえば、今回の例だとIAuthorQiitaPostインターフェイスを設計するときに、「あれ、削除するだけじゃなくて編集する機能もいるな」と気づきやすくなる
*/
public class QiitaPost_WithInterface : IQiitaPost, IReaderQiitaPost, IAuthorQiitaPost
{
    string _title;
    string _text;

    // タイトル と 本文 を入力して記事を作成
    public QiitaPost_WithInterface(string title, string text)
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
        ++LGTMCount;
    }
    // ストックするメソッド
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
