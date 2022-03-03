using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* どのように シーン を切り替えるか

既知の問題 :
    現在, LightMap など Scene に紐づいた DataMap はない
    しかし、シーンを切り替えたとき、RealTime Light である DirectionalLight の強度が下がったように Object を照らしてしまう
    Intensity が 1 の時に、違いが顕著に現れるのだが

Scenes In Build には(Build Setting に登録されているシーン)
0 : MultipleScene_A
1 : MultipleScene_B

「 Scene A => Scene B に移動するときの ロード・アンロード方法について考える。」

手順 パターン１
1. Scene B をロード
2. ロード完了後、 B を ActiveScene に指定
3. Scene A をアンロード

手順 パターン2
1. Scene A をアンロード
2. アンロード完了後、 B を ロード
2. ロード完了後、 B を ActiveScene に指定

解決策 :
これは、コードの記述自体に間違いはなっかた。
手順は以下の通りである。

1. Window > Rendering > Lightning > scene で Lighting Setting に何も設定されていないか確認する。

2. None だった場合は、Lighting Setting を設定する or 新たに生成する(New Lighting Settings ボタンを押す)。

3. 生成後、Generate Lighting を押して LightingData の生成ができれば完了

これで、 ActiveScene が切り替わった時も大丈夫!

*/
namespace MultipleScenes
{
    public class MySceneManager1 : MonoBehaviour
    {
        const string SceneA = "MultipleScene_A";
        const string SceneB = "MultipleScene_B";

        void Start()
        {
            DontDestroyOnLoad(this);
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                LoadPattern1();
                // LoadPattern2();
            }
        }

        void LoadPattern1()
        {
            var loadB = SceneManager.LoadSceneAsync(SceneB, LoadSceneMode.Additive);
            StartCoroutine(LoadRoutine());

            IEnumerator LoadRoutine()
            {
                yield return loadB;
                // yield return null;
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneB));
                yield return SceneManager.UnloadSceneAsync(SceneA);
            }
        }
        void LoadPattern2()
        {
            var unLoadA = SceneManager.UnloadSceneAsync(SceneA);
            StartCoroutine(LoadRoutine());

            IEnumerator LoadRoutine()
            {
                yield return unLoadA;
                yield return new WaitForSeconds(3.0f);
                yield return SceneManager.LoadSceneAsync(SceneB);
                yield return new WaitForSeconds(3.0f);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneB));
            }
        }
    }
}
