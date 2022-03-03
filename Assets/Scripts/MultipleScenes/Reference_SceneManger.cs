using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;
using UnityEngine.SceneManagement;

/* SceneManager で可能なことについて

SceneManager (実行時のシーン管理) が 出来ることについて検証していく

Static 変数
1. sceneCount : 現在読み込まれているシーンの数
2. sceneCountInBuildSettings : ビルド設定にあるシーンの数

Static 関数
3. CreateScene : 新しい Scene の名前を作成する。現在読み込まれているシーンに追加して開かれる
4. GetActiveScene : SceneManager のロードされたシーン一覧のインデックスにあるシーンを取得
5. GetSceneAt: SceneManager のロードされたシーン一覧のインデックスにあるシーンを取得
6. GetSceneByBuildIndex: Build Settings ウィンドウで表示されるビルドインデックス で Scene を取得
7. GetSceneByName : 読み込まれたシーンの中から、指定された名前のシーンを検索する
8. 
*/

public class Reference_SceneManger : MonoBehaviour
{
    void Start()
    {
        // 1.
        // Debug.Log(sceneCount);

        // 2.
        // Debug.Log(sceneCountInBuildSettings);

        // 3.
        // Scene newScene = CreateScene("My New Scene");

        // 4.
        // Scene scene = SceneManager.GetActiveScene();
        // Debug.Log($"Active Scene is {scene.name}");

        // 5.
        // Debug.Log(GetSceneAt(sceneCount));

        // 6.
        // Debug.Log(GetSceneByBuildIndex(0).name);

        // 7
        // GetSceneByName(GetSceneByBuildIndex(0).name);

    }

    void Update()
    {
        
    }


}
