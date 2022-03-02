using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;
using UnityEngine.SceneManagement;

/* Unity における 複数シーンの扱い
Unitで複数シーンの扱い、よりよい設計ができるようになることが目標。

シーンロードに関わること、複数シーン扱うときに使えそうなものを記述していく。



(なぜか、 名前空間 に using UnityEngine.SceneManagement; 記述しても
SceneManager.LoadSccne(); が SceneManagement を認識しない。
クラス名が SceneManager にしてしまっているかだ。

using static ディレクティブ を使用すれば問題ないが 後のことも考えて クラス名を MySceneManager に変更した)


Scenes In Build には(Build Setting に登録されているシーン)
0 : MultipleScene_A
1 : MultipleScene_B

が設定されている。

1. 同期的なシーンロード
2. 同期的なシーン追加
3. 非同期的なシーンのアンロード
4. 非同期的な不使用アセットのアンロード
5. ActiveScene の切り替え

*/

namespace MultipleScenes
{
    public class MySceneManager : MonoBehaviour
    {
        void Start()
        {
            // シーンをまたいで消さないオブジェクト
            // DontDestroyOnLoad(this);


            // LoadSingleScene(1);
            AddLoadedScene(1);
            Debug.Log(SceneManager.GetActiveScene().name);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                UnLoadScene(0);
            }
        }


        void LoadSingleScene(int num)
        {
            // 1. 同期的にシーンをロード
            LoadScene(num, LoadSceneMode.Single);
        }

        void AddLoadedScene(int num)
        {
            // 2. シーンの追加
            LoadScene(num, LoadSceneMode.Additive);
        }

        void UnLoadScene(int num)
        {
            // 3. シーンのアンロード
            var unLoad =  UnloadSceneAsync(num);
            StartCoroutine(UnLoadSceneRoutine());

            IEnumerator UnLoadSceneRoutine()
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("MultipleScene_B"));
                Debug.Log(SceneManager.GetActiveScene().name);
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                yield return unLoad;
                // アンロード完了後の処理
                Debug.Log($"シーンのアンロード完了 : {sw.ElapsedMilliseconds}");

                // 4. 不使用アセットをアンロード
                // LoadSceneMode.Additiveの場合、SceneManager.UnloadSceneAsyncでシーンをアンロードした場合も、不使用アセットのアンロードは行われない
                // 必要に応じて不使用アセットをアンロードしてメモリを解放する けっこう重い処理なので、別に管理するのもあり
                // LoadSceneMode.Singleの場合は自動
                // ただし、メモリに余裕があるなら、リソースはアンロードしないほうが次に使うときに再ロードする必要なくすぐ使えるというメリットもある
                yield return Resources.UnloadUnusedAssets();
                Debug.Log($"不使用アセットのアンロード完了 : {sw.ElapsedMilliseconds}");

                // 5. ActiveSceneの切り替え
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("MultipleScene_B"));
            }
        }
    }
}
