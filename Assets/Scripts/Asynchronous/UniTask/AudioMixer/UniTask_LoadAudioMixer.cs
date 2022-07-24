using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;

public class UniTask_LoadAudioMixer : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    const string AudioMixerPath = "Common/SampleAudioMixer";
    // Start is called before the first frame update
    // async UniTask Start()
    // {
    //     // LoadAudioMixer();
    //     _audioMixer = await LoadAudioMixerUniTask();
    // }
    void Start()
    {
        // LoadAudioMixer();
        LoadAudioMixerUniTask2().Forget();
    }

    // 同期処理
    void LoadAudioMixer()
    {
        _audioMixer = Resources.Load<AudioMixer>(AudioMixerPath);
    }

    // 非同期処理の記述
    async UniTask<AudioMixer> LoadAudioMixerUniTask()
    {
        return await Resources.LoadAsync<AudioMixer>(AudioMixerPath) as AudioMixer;
    }
    async UniTask LoadAudioMixerUniTask2()
    {
        _audioMixer = await Resources.LoadAsync<AudioMixer>(AudioMixerPath) as AudioMixer;
    }
}
