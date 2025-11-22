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
    [SerializeField] private SoundLibrarySO _library;
    [SerializeField] private ESoundType _initialBgmType = ESoundType.BGM1;
    [SerializeField] private GameObject _bgmObject;

    [Header("BGM Settings")]
    [SerializeField] private string _bgmObjectName = "BGM";
    private SoundObject _bgmSource;

    private PoolManager _pool => PoolManager.Instance;
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
        if (_bgmObject == null)
        {
            Debug.Log("[SoundManager] Created BGM AudioSource.");

            GameObject newObject = new GameObject(_bgmObjectName);
            newObject.transform.SetParent(transform);

            _bgmSource = newObject.AddComponent<SoundObject>();
        }
        else
        {
            if (!_bgmObject.TryGetComponent<SoundObject>(out _bgmSource))
            {
                _bgmSource = _bgmObject.AddComponent<SoundObject>();
            }
        }

        AudioClip targetAudio = _library.GetClip(_initialBgmType);
        if (targetAudio != null)
        {
            _bgmSource.OnPlay(targetAudio, true);
        }
    }


    public void PlayBGM(ESoundType bgmType, float playTime = 0)
    {
        if (_bgmSource == null || _library == null)
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
            Debug.LogWarning($"SoundManager: There's no Sound library");
            return;
        }

        var clip = _library.GetClip(type);
        if (clip == null)
        {
            Debug.LogWarning($"SoundManager: Clip can't find in library");
            return;
        }

        
        SoundObject soundObject = _pool.SpawnGetComponent<SoundPool, ESoundObject, SoundObject>(ESoundObject.SoundObject);
        if (soundObject == null)
        {
            Debug.LogWarning($"SoundManager: PoolObject does't have SoundObject Component");
            return;
        }

        soundObject.OnPlaybackFinished = () =>
        {
            _pool.Despawn<SoundPool, ESoundObject>(ESoundObject.SoundObject, soundObject.gameObject);
        };

        soundObject.OnPlay(clip, false, playTime);
    }
}
