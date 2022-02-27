using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QittaAutor : MonoBehaviour
{
    void PostQiitaNoInterface()
    {
        // インターフェースを介さず 直接クラスにアクセルして 記事を作成
        QiitaPost_NoInterface post = new QiitaPost_NoInterface("タイトル", "本文");

        // 記事をアップロードする処理
        // QiitaServer.Upload(post);

        // 自分の記事にアクセスできてしまう
        post.LGTM();
    }
    void PostQiitaWithInterface()
    {
        // インターフェースを介して 記事を作成
        IAuthorQiitaPost post = new QiitaPost_WithInterface("タイトル", "本文");

        // 記事をアップロードする処理
        // QiitaServer.Upload(post);

        // IAuthorQiitaPostインターフェイスのメンバに、LGTMメソッドが存在しないので
        // 自分の記事にLGTMできない
        // post.LGTM();

        // 下記のような記述をしてしまったら、インターフェースを介さず、アクセスできてしまうけど...
        // QiitaPost_WithInterface post1 = new QiitaPost_WithInterface("タイトル", "本文");
        // post1.LGTM();
    }
}
