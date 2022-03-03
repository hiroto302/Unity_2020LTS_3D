using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cysharp.Threading.Tasks;


/* どのように シーンを切り替えるか MySceneManager2 の続き

Scene A => Scene B に移動するときの ロード・アンロード方法について
Scene B を ロードしている間は ロード画面(Loading Scene) を挟む。
シーンが切り替わる場面(瞬間) は ユーザーには見せない。

また、コルーチンではなく UniTask, Fade 処理には DOTween、event 処理には UniRx を用いて実装する

Scenes In Build には(Build Setting に登録されているシーン)
0 : MultipleScene_A
1 : MultipleScene_B
2 : MultipleScene_Loading

初期画面には、MultipleScene_A と MultipleScene_Loading が存在する

手順 パターン１をアップグレード 処理を各クラスに分ける。
ロード処理を開始することを、各クラスにどのように伝えるかがキモ
ここでは、「Singleton」の仕組みを利用。この実装方法を変えていきたい。
何か、ハブ的な存在、仲介者のようなものが欲しい。オブジェクトやシーンに依存していないもの

1. ロードメソッド が実行される
2. Scene A は、LoadingScene にある Image を利用して FadeIn 開始
3. FadeIn が完了 (Image だけじゃなく Sound とか他の処理も実行完了を待つことを想定して実装すること)
4. Scene B を非同期でロード開始 ( Dummy Camera を表示してもいいかも, MainCamera は勝手に切り替わるからゲーム内容による)
5. ロード完了後、 B を ActiveScene に指定
6. Scene A をアンロード
7. LoadingScene にある Image を利用して FadeOut 開始

ロード処理でやりたいことは出来ることが判明した。
繰り返しになるが、問題は、シーンを跨いだオブジェクト同士のやり取りをどのように行うかである。
方法として、

1. シングルトン (シーンに依存)
2. スクリプタブルオブジェクト(シーンに依存しない)

が考えられる。模索していく。

*/

namespace MultipleScenes
{
    public class MySceneManager3 : MonoSingleton<MySceneManager3>
    {
        const string SceneA = "MultipleScene_A";
        const string SceneB = "MultipleScene_B";



        void Start()
        {
            DontDestroyOnLoad(this);
            // _uIManager = GameObject.Find;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                LoadScene(SceneA);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                LoadScene(SceneB);
            }
        }

        async void LoadScene(string sceneName)
        {
            Debug.Log("ロード処理開始");
            // fade 処理完了まで待機
            await UniTask.WhenAll(UIManager.Instance.FadeInAlphaUniTask(3.0f));
            // DummyCamera On
            UIManager.Instance.SetDummyCamera(true);
            // fade 処理完了後 シーンを追加
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // 現在のActive シーンをアンロード と 3000 ミリ秒待機
            await UniTask.WhenAll(
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()).ToUniTask(),
                UniTask.Delay(3000)
                );

            // Active Scene を変更
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            // DummyCamera Off
            UIManager.Instance.SetDummyCamera(false);
            // fade 処理完了まで待機
            await UniTask.WhenAll(UIManager.Instance.FadeOutAlphaUniTask(3.0f));
            Debug.Log("ロード処理完了");
        }
    }
}
