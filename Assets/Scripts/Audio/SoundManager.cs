using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float musicVol, effectsVol, MasterVol;
    public static SoundManager Instance;
    private float timer;
    [Serializable]
    public struct PrefabData
    {
        [Tooltip("音乐名字")]
        public string _clipName;
        [Tooltip("音乐资源")]
        public AudioClip _clipSource;
    }
    [Tooltip("可以使用字符串绑定相应的音乐资源，方便统一调用和录入")]
    public List<PrefabData> _soundClipList = new List<PrefabData>();
    public GameObject _soundPrefab;

    public Dictionary<string, AudioClip> soundClip = new Dictionary<string, AudioClip>();
    private Dictionary<AudioClip, float> soundTime = new Dictionary<AudioClip, float>();

    private void Start()
    {
        // 字典内容添加
        for (int i = 0; i < _soundClipList.Count; i++)
        {
            soundClip.Add(_soundClipList[i]._clipName, _soundClipList[i]._clipSource);
            soundTime.Add(_soundClipList[i]._clipSource, 0f);
        }
        musicVol = _musicSource.volume;
        effectsVol = _effectsSource.volume;
        MasterVol = _MasterSource.volume;
    }


    [SerializeField] private AudioSource _musicSource, _effectsSource, _MasterSource, x16, x8, x4, x2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void MusicPlayClip(AudioClip clip)
    {
        //if (_musicSource.clip != null) soundTime[_musicSource.clip] = _musicSource.time;
        _musicSource.Stop();
        _musicSource.clip = clip;
        _musicSource.Play();
        //_musicSource.time = soundTime[clip];
    }
    public void PlaySound()
    {
        x16.volume = _musicSource.volume;
        x8.volume = _musicSource.volume;
        x4.volume = _musicSource.volume;
        x2.volume = _musicSource.volume;
        x16.Play();
        x8.Play();
        x4.Play();
        x2.Play();
    }
    public void MuteSound(int ratio)
    {
        switch (ratio)
        {
            case 16:
                x16.mute = true; break;
            case 8:
                x8.mute = true; break;
            case 4:
                x4.mute = true; break;
            case 2:
                x2.mute = true; break;
            default: break;
        }
    }
    public void ReleaseMuteSound(int ratio)
    {
        switch (ratio)
        {
            case 16:
                x16.mute = false; break;
            case 8:
                x8.mute = false; break;
            case 4:
                x4.mute = false; break;
            case 2:
                x2.mute = false; break;
            default: break;
        }
    }
    public void StopSound()
    {
        x16.Stop();
        x8.Stop();
        x4.Stop();
        x2.Stop();
    }
    public void EffectPlayClip(AudioClip clip)
    {
        _effectsSource.Stop();
        _effectsSource.PlayOneShot(clip);
    }
    public void SceneEffectPlayClip(AudioClip clip)
    {
        GameObject audioSource = Instantiate(_soundPrefab);
        audioSource.GetComponent<AudioSource>().volume = _effectsSource.volume;
        audioSource.GetComponent<AudioSource>().mute = _effectsSource.mute;
        audioSource.GetComponent<AudioSource>().PlayOneShot(clip);
        Destroy(audioSource, 5.0f);
    }
    public void MusicStop()
    {
        //timer = _musicSource.time;
        _musicSource.Stop();
    }
    public void MusicPlay()
    {
        //_musicSource.time = timer;
        _musicSource.Play();
    }
    public void ChangeVolumeMusic(float value)
    {
        _musicSource.volume = value;
        x16.volume = value;
        x8.volume = value;
        x4.volume = value;
        x2.volume = value;
        musicVol = value;
    }
    public float ReturnVolumeMusic()
    {
        return musicVol;
    }

    public void ChangeVolumeEffect(float value)
    {
        _effectsSource.volume = value;
        effectsVol = value;
    }
    public float ReturnVolumeEffect()
    {
        return effectsVol;
    }
    public void ChangeVolumeMaster(float value)
    {
        _MasterSource.volume = value;
        MasterVol = value;
    }
    public float ReturnVolumeMaster()
    {
        return MasterVol;
    }

    public void ToggleEffects()
    {
        _effectsSource.mute = !_effectsSource.mute;
    }
    public void ToggleMusic()
    {

        _musicSource.mute = !_musicSource.mute;
    }

    public void MusicPlayStr(string str)
    {
        if (soundClip.ContainsKey(str)) MusicPlayClip(soundClip[str]);
    }
    public void EffectPlayStr(string str)
    {
        if (soundClip.ContainsKey(str)) EffectPlayClip(soundClip[str]);
    }
    public void SceneEffectPlayStr(string str)
    {
        if (soundClip.ContainsKey(str)) SceneEffectPlayClip(soundClip[str]);
    }
    public float returnTime()
    {
        return _musicSource.time;
    }
}
