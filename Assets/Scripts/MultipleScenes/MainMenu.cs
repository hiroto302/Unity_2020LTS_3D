using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cysharp.Threading.Tasks;


/* MainMenu の実装について
アプリを起動後、Player が 始めに見るシーンにある MainMenu を実装する


*/
namespace MultipleScenes
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] GameObject _menu = null;
        [SerializeField] GameObject _loadingInterface = null;
        [SerializeField] Image _loadingProgressBar = null;
        // メインメニューから読み込むシーンの一覧
        List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();

        public void StartGame()
        {
            HideMenu();
            ShowLoadingScreen();
            // 読み込むシーンを追加
            _scenesToLoad.Add(SceneManager.LoadSceneAsync("MultipleScene_A"));
            _scenesToLoad.Add(SceneManager.LoadSceneAsync("MultipleScene_Loading", LoadSceneMode.Additive));
            StartCoroutine(LoadingScreenRoutine());

            IEnumerator LoadingScreenRoutine()
            {
                float totalProgress = 0;
                for(int i = 0; i < _scenesToLoad.Count; ++i)
                {
                    while (!_scenesToLoad[i].isDone)
                    {
                        totalProgress += _scenesToLoad[i].progress;
                        _loadingProgressBar.fillAmount = totalProgress / _scenesToLoad.Count;
                        yield return new WaitForSeconds(2.0f);
                    }
                }
            }
        }

        void HideMenu()
        {
            _menu.SetActive(false);
        }
        void ShowLoadingScreen()
        {
            _loadingInterface.SetActive(true);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
