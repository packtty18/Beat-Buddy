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
    private ESoundType _currentBgm = ESoundType.None;


    protected override void Awake()
    {
        base.Awake();

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


    public void PlayBGM(ESoundType bgmType)
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

        _currentBgm = bgmType;
        Debug.Log($"[SoundManager] PlayBGM requested: {bgmType}");

        _bgmSource.OnPlay(clip, true);
    }

    public void StopBGM()
    {
        _bgmSource.StopPlay();
        _currentBgm = ESoundType.None;
    }



    public void PlaySFX(ESoundType type)
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
        so.OnPlay(clip, false);
    }

    #region Debug Variables
    [SerializeField]
    private ESoundType _debugSelectedType = ESoundType.None;
    [SerializeField]
    private bool _debugIsBGM = false;
    [SerializeField]
    private bool _debugAutoPlay = false;

    [ContextMenu("Play Debug Sound")]
    public void PlayDebugSound()
    {
        if (_library == null)
        {
            Debug.LogWarning("[SoundManager Debug] SoundLibrary is missing!");
            return;
        }

        if (_debugSelectedType == ESoundType.None)
        {
            Debug.LogWarning("[SoundManager Debug] No sound type selected!");
            return;
        }

        AudioClip clip = _library.GetClip(_debugSelectedType);
        if (clip == null)
        {
            Debug.LogWarning($"[SoundManager Debug] Clip not found for {_debugSelectedType}");
            return;
        }

        if (_debugIsBGM)
        {
            // BGM용 사운드 오브젝트 생성 혹은 변경
            if (_bgmSource == null)
            {
                SetBgmSource();
            }
            _currentBgm = _debugSelectedType;
            _bgmSource.OnPlay(clip, true);
        }
        else
        {
            GameObject soundObject = PoolManager.Instance.Spawn<SoundPool, ESoundObject>(ESoundObject.SoundObject);
            var so = soundObject.GetComponent<SoundObject>();
            so.OnPlay(clip, false);
        }
    }
    #endregion

}
