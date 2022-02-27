using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QittaReader : MonoBehaviour
{
    void ReadNoInterface()
    {
        // Qiita記事を取得
        // QiitaPost_NoInterface downloadedPost = QiitaServer.Download("https://~");
        // 所得した仮のインスタンス
        QiitaPost_NoInterface downloadedPost = new QiitaPost_NoInterface("タイトル","本文");

        //取得した記事のタイトルと本文を読む
        string title = downloadedPost.Title;
        string text = downloadedPost.Text;
        //記事をLGTMする
        downloadedPost.LGTM();

        //勝手に人の書いた記事を消せてしまう
        downloadedPost.Delete();
    }

    void ReadWithInterface()
    {
        // Qiita記事を取得
        // IReaderQiitaPost downloadedPost = QiitaServer.Download("https://~");
        // 所得した仮のインスタンス
        IReaderQiitaPost downloadedPost = new QiitaPost_WithInterface("タイトル","本文");

        //取得した記事のタイトルと本文を読む
        string title = downloadedPost.Title;
        string text = downloadedPost.Text;
        //記事をLGTMする
        downloadedPost.LGTM();

        //勝手に人の書いた記事を消せない〜〜〜
        // downloadedPost.Delete();
    }
}
