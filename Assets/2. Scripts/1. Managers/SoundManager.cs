using System;
using System.Collections;
using UnityEngine;
public enum ESoundType
{
    None = 0,
    BGM1,
    BGM2,
    BGM3,
    BGM4,
    SFX1,
}
public class SoundManager : SimpleSingleton<SoundManager>
{
    [Header("사운드 라이브러리 추가")]
    [SerializeField] private SoundLibrary _library;

    [Header("BGM Settings")]
    [SerializeField] private string _bgmObjectName = "BGM";
    private SoundObject _bgmSource;


    protected override void Awake()
    {
        base.Awake();

        Init();
    }

    public void Init()
    {
        if (_library == null)
        {
            Debug.LogError("SoundManager: SoundLibrary reference is missing!");
            return;
        }

        _library.InitSoundMap();
        SetBgmSource();
    }

    private void SetBgmSource()
    {
        GameObject bgmObject = GameObject.Find(_bgmObjectName);
        //BGM오브젝트를 찾지 못하면 새로 하나 만들기
        if (bgmObject == null)
        {
            Debug.Log("[SoundManager] Created BGM AudioSource.");
            GameObject newObject = new GameObject(_bgmObjectName);
            newObject.transform.SetParent(transform);
            _bgmSource = newObject.AddComponent<SoundObject>();
        }
        //존재하면 해당 오브젝트 선정
        else
        {
            _bgmSource = bgmObject.GetComponent<SoundObject>();
        }

        AudioClip targetAudio = _library.GetClip(ESoundType.BGM1);
        _bgmSource.OnPlay(targetAudio, true);
    }


    public void PlayBGM(ESoundType bgmType , float playTime = 0)
    {
        if (_library == null)
        {
            return;
        }

        if (bgmType == ESoundType.None)
        {
            return;
        }

        var clip = _library.GetClip(bgmType);
        if (clip == null)
        {
            Debug.LogWarning($"SoundManager:  {bgmType} clip not found in library");
            return;
        }

        Debug.Log($"[SoundManager] PlayBGM requested: {bgmType}");

        _bgmSource.OnPlay(clip, true, playTime);
    }

    public void StopBGM()
    {
        _bgmSource.StopPlay();
    }



    public void PlaySFX(ESoundType type, float playTime = 0)
    {
        if (_library == null)
        {
            return;
        }

        var clip = _library.GetClip(type);
        if (clip == null)
        {
            return;
        }

        GameObject soundObject = PoolManager.Instance.Spawn<SoundPool, ESoundObject>(ESoundObject.SoundObject);
        var so = soundObject.GetComponent<SoundObject>();
        so.OnPlay(clip, false, playTime);
    }

    public SoundLibrary GetLibrary()
    {
        return _library;
    }

    public SoundObject GetBgmSource()
    {
        return _bgmSource;
    }
}
