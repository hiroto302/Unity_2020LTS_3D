using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniTask_SettingView : MonoBehaviour
{
    const string Key = "SaveKey";

    void Start()
    {
        SaveValue(Key, 11.0f);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("ロードした値 : " + LoadedValue(Key).ToString());
        }
    }

    // 保存処理
    void SaveValue(string key, float savedValue)
    {
        PlayerPrefs.SetFloat(key, savedValue);
    }

    // ロード処理
    float LoadedValue(string key)
    {
        return PlayerPrefs.GetFloat(key, 0f);
    }

    // 上記のロード処理を 非同期処理で記述したい
    
}
