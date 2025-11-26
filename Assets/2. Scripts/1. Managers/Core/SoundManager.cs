using System;
using System.Collections;
using UnityEngine;
public enum ESoundType
{
    None = 0,
    BGM_Start =1,
    BGM_ModeSelect=2,
    BGM_StageSelect=3,
    BGM_Result=4,
    BGM_Stage1=5,
    BGM_Stage2=6,
    BGM_Stage3=7,
    BGM_Stage4=8,
    BGM_Stage5=9,
    SFX_PressAnyButton=10,
    SFX_SongSelected=11,
    SFX_HitDrum=12,
    SFX_HitClap=13,
    SFX_NoteMiss=14,
    SFX_ButtonConfirm=15,
    SFX_ButtonSelect=16,
    SFX_SettingUIAppear=17,
    SFX_FeverEnd=18,
    SFX_PlayerAttack=19,
    SFX_PlayerHit=20,
    SFX_Upgrade=21,
    SFX_StageFail=22,
    SFX_StageVictory=23,
}
public class SoundManager : CoreSingleton<SoundManager>
{
    [Header("사운드 라이브러리 추가")]
    [SerializeField] private SoundDatabaseSO _soundDatabase;
    [SerializeField] private ESoundType _initialBgmType = ESoundType.BGM_Start;
    [SerializeField] private GameObject _bgmObject;

    [Header("BGM Settings")]
    [SerializeField] private string _bgmObjectName = "BGM";
    private SoundObject _bgmSource;

    private float _bgmVolume = 1f;
    private float _sfxVolume = 1f;

    public float bgmVolume => _bgmVolume;
    public float SfxVolume => _sfxVolume;

    protected override void Awake()
    {
        base.Awake();

        Init();
    }

    public void Init()
    {
        if (_soundDatabase == null)
        {
            Debug.LogError("SoundManager: SoundLibrary reference is missing!");
            return;
        }

        SetBgmSource();
    }

    public void SetBgmSound(float value)
    {
        _bgmVolume = value;
        _bgmObject.GetComponent<SoundObject>().SetVolume(value);
    }

    public void SetSFXSound(float value)
    {
        _sfxVolume = value;
    }

    private void SetBgmSource()
    {
        if (_bgmObject == null)
        {
            Debug.Log("[SoundManager] Created BGM AudioSource.");

            GameObject newObject = new GameObject(_bgmObjectName);
            newObject.transform.SetParent(transform);
            _bgmObject = newObject;
            _bgmSource = newObject.AddComponent<SoundObject>();
        }
        else
        {
            if (!_bgmObject.TryGetComponent(out _bgmSource))
            {
                _bgmSource = _bgmObject.AddComponent<SoundObject>();
            }
        }

        AudioClip targetAudio = _soundDatabase.GetData(_initialBgmType);
        if (targetAudio != null)
        {
            _bgmSource.OnPlay(targetAudio, true, bgmVolume);
        }
    }

    public void PlayBGM(AudioClip clip, float playTime = 0)
    {
        if (_bgmSource == null || _soundDatabase == null)
        {
            return;
        }

        _bgmSource.OnPlay(clip, true, bgmVolume, playTime);
    }

    public void PlayBGM(ESoundType bgmType, float playTime = 0)
    {
        if (_bgmSource == null || _soundDatabase == null)
        {
            return;
        }

        if (bgmType == ESoundType.None)
        {
            return;
        }

        var clip = _soundDatabase.GetData(bgmType);
        if (clip == null)
        {
            Debug.LogWarning($"SoundManager:  {bgmType} clip not found in library");
            return;
        }

        Debug.Log($"[SoundManager] PlayBGM requested: {bgmType}");

        _bgmSource.OnPlay(clip, true,bgmVolume, playTime);
    }

    public void StopBGM()
    {
        _bgmSource.StopPlay();
    }



    public void PlaySFX(ESoundType type, float playTime = 0)
    {
        if (_soundDatabase == null)
        {
            Debug.LogWarning($"SoundManager: There's no Sound library");
            return;
        }

        var clip = _soundDatabase.GetData(type);
        if (clip == null)
        {
            Debug.LogWarning($"SoundManager: Clip can't find in library");
            return;
        }

        if(!PoolManager.IsManagerExist())
        {
            Debug.LogWarning($"SoundManager: PoolManager Instance is null");
            return;
        }

        PoolManager _pool = PoolManager.Instance;

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

        soundObject.OnPlay(clip, false, SfxVolume, playTime);
    }
}
