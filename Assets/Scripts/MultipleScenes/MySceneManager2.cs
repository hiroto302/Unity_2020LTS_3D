using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cysharp.Threading.Tasks;


/* どのように シーンを切り替えるか

問題であった、 ActiveScene の LightingData を正しく表示することができるようになった。
次に、以下のように シーンを切り替えることを考える

Scene A => Scene B に移動するときの ロード・アンロード方法について
Scene B を ロードしている間は ロード画面(Loading Scene) を挟む。
シーンが切り替わる場面(瞬間) は ユーザーには見せない。

また、コルーチンではなく UniTask, Fade 処理には DOTween、event 処理には UniRx を用いて実装する

Scenes In Build には(Build Setting に登録されているシーン)
0 : MultipleScene_A
1 : MultipleScene_B
2 : MultipleScene_Loading

初期画面には、MultipleScene_A と MultipleScene_Loading が存在する

手順 パターン１ とりあえず、この MySceneManger クラスだけで 完結した処理を書く
1. ロードメソッド が実行される
2. Scene A は、同じシーンにある Image を利用して FadeIn 開始
3. FadeIn が完了 (Image だけじゃなく Sound とか他の処理も実行完了を待つことを想定して実装すること)
4. Scene B を非同期でロード開始 ( Dummy Camera を表示してもいいかも, MainCamera は勝手に切り替わるからゲーム内容による)
5. ロード完了後、 B を ActiveScene に指定
6. Scene A をアンロード
7. Image を利用して FadeOut 開始

下記の実装を MySeneManger3 実装する

手順 パターン１をアップグレード 処理を各クラスに分ける。 ロード処理を開始することを、各クラスにどのように伝えるかがキモ
1. ロードメソッド が実行される
2. Scene A は、LoadingScene にある Image を利用して FadeIn 開始
3. FadeIn が完了 (Image だけじゃなく Sound とか他の処理も実行完了を待つことを想定して実装すること)
4. Scene B を非同期でロード開始 ( Dummy Camera を表示してもいいかも, MainCamera は勝手に切り替わるからゲーム内容による)
5. ロード完了後、 B を ActiveScene に指定
6. Scene A をアンロード
7. LoadingScene にある Image を利用して FadeOut 開始

*/

namespace MultipleScenes
{
    public class MySceneManager2 : MonoBehaviour
    {
        const string SceneA = "MultipleScene_A";
        const string SceneB = "MultipleScene_B";

        // UI 処理は分けるべきだが、とりあえずまとめて書く
        [SerializeField] Image _image = null;


        void Start()
        {
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(_image.transform.root.gameObject);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                // LoadSceneBWithCoroutine();
                LoadSceneBWithUniTask();
            }
        }

        void LoadSceneBWithCoroutine()
        {
            // 下記の処理を UniTask に置き換える
            StartCoroutine(LoadRoutine());
            IEnumerator LoadRoutine()
            {
                yield return FadeInAlphaTween(3.0f).WaitForCompletion();
                yield return SceneManager.LoadSceneAsync(SceneB, LoadSceneMode.Additive);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneB));
                yield return SceneManager.UnloadSceneAsync(SceneA);
                yield return FadeOutAlphaTween(3.0f).WaitForCompletion();
            }
        }

        async void LoadSceneBWithUniTask()
        {
            Debug.Log("ロード処理開始");
            // fade 処理完了まで待機
            await UniTask.WhenAll(FadeInAlphaUniTask(3.0f));
            await SceneManager.LoadSceneAsync(SceneB, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneB));
            await SceneManager.UnloadSceneAsync(SceneA);
            await UniTask.WhenAll(FadeOutAlphaUniTask(3.0f));
            Debug.Log("ロード処理完了");
        }

        // UniTask fade 処理
        async UniTask FadeInAlphaUniTask(float duration)
        {
            await _image.DOFade(endValue: 1, duration).SetEase(Ease.Linear);
        }
        async UniTask FadeOutAlphaUniTask(float duration)
        {
            await _image.DOFade(endValue: 0, duration).SetEase(Ease.Linear);
        }

        // DoTween を用いた fade 処理
        Tween FadeInAlphaTween(float duration)
        {
            return _image.DOFade(endValue: 1, duration).SetEase(Ease.Linear);
        }
        Tween FadeOutAlphaTween(float duration)
        {
            return _image.DOFade(endValue: 0, duration).SetEase(Ease.Linear);
        }
    }
}
