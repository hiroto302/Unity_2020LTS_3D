using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

/* Image と RawImage の違いについて
Imageコンポーネントでは画像ファイルにスプライトを使う。
スプライトとは、画像のアセットのTexture TypeをSprite (2D and UI)にしてあるもの。

RawImageコンポーネントを使用するには、画像のアセットのTexture TypeをTextureもしくはAdvancedにする。
こいつのメリットは、ゲーム中に作成、取得した画像を表示できる
RawImageに設定するTextureというクラスはとても柔軟で、プログラム中で容易に作成したり内容を変更が可能。
また、ネットワークから取得した画像の表示なども簡単。
極端な話、画像アセットを用意しなくても、プログラムだけでRawImageに画像を表示できます。
*/
public class Coroutine_Sample : MonoBehaviour
{
    // ネットから拾ってくる画像 先のリンク
    const string URI = "https://4.bp.blogspot.com/-4xxTe_qeV1E/Vd7FkNUlwjI/AAAAAAAAxFc/8u9MNKtg7gg/s800/syachiku.png";

    void Start()
    {
        var image = GetComponent<RawImage>();
        StartCoroutine(GetTexture2D(t => image.texture = t));
    }

    IEnumerator GetTexture2D(Action<Texture2D> onResult)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URI);
        // 画像を取得できるまで待機
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            // 結果はコールバックで取得
            onResult(texture);
        }
    }
}
